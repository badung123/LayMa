using AutoMapper;
using LayMa.Core.Domain.Identity;
using LayMa.Core.Domain.Link;
using LayMa.Core.Interface;
using LayMa.Core.Model;
using LayMa.Core.Model.ShortLink;
using LayMa.WebAPI.Extensions;
using LayMa.WebAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using System.Reflection.Metadata.Ecma335;
using static LayMa.Core.Constants.Permissions;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace LayMa.WebAPI.Controllers.AdminApi
{
	[Route("api/admin/shortlink")]
	[ApiController]
	public class ShortLinkController : ControllerBase
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly UserManager<AppUser> _userManager;
		private readonly IMapper _mapper;
		private readonly IShortLinkService _shortLinkService;
		public ShortLinkController(IUnitOfWork unitOfWork, UserManager<AppUser> userManager ,IMapper mapper, IShortLinkService shortLinkService)
        {
            _mapper = mapper;
			_unitOfWork = unitOfWork;
			_userManager = userManager;
			_shortLinkService = shortLinkService;
        }		
		[HttpPost]
		[Route("create")]
		//[Authorize(ShortLinks.Create)]
		public async Task<IActionResult> CreateShortLink([FromBody] CreateShortLinkDto request) {
			var userId = User.GetUserId();
			var user = await _userManager.FindByIdAsync(userId.ToString());
			var link = _mapper.Map<CreateShortLinkDto,ShortLink>(request);
			link.DateCreated = DateTime.Now;
			link.DateModified = DateTime.Now;
			link.UserId = userId;
			var id = Guid.NewGuid();
			link.Id = id;
			link.OriginLink = request.Url;
			var token = _shortLinkService.GenerateLinkToken();
			link.Token = token;
			link.Link = "https://layma.net/" + token;
			_unitOfWork.ShortLinks.Add(link);
			var result = await _unitOfWork.CompleteAsync();
			return result > 0 ? Ok() : BadRequest();
		}

		[HttpGet]
		[Route("paging")]
		//[Authorize(ShortLinks.View)]
		public async Task<ActionResult<PagedResult<ShortLinkInListDto>>> GetPostsPaging(int pageIndex, int pageSize = 10, string? keySearch="")
		{
			var userId = User.GetUserId();
            var result = await _unitOfWork.ShortLinks.GetAllPaging(userId, pageIndex, pageSize,keySearch);
			return Ok(result);
		}
		[HttpGet]
		[Route("gettop")]
		public async Task<ActionResult<List<ShortLinkInListDto>>> GetTopLink()
		{
			var userId = User.GetUserId();
			var rs = await _unitOfWork.ShortLinks.GetTopLink(userId, 10);
			return Ok(rs);
		}
		[HttpGet]
		[Route("getbalance")]
		public async Task<ActionResult<int>> GetBalance()
		{
			var userId = User.GetUserId();
			var user = await _userManager.FindByIdAsync(userId.ToString());
			if (user == null) return BadRequest("User không tồn tại");
			return Ok(user.Balance);
		}

        [HttpGet]
        [Route("thongkeClickByDate")]
        public async Task<ActionResult<int>> GetThongKeClickByDate()
        {
			var countClick = 0;
            var userId = User.GetUserId();
			var date = DateTime.Now;
            var start = new DateTime(date.Year, date.Month, 1);
            var end = DateTime.Now;
			//get list token short link
			var listTokenShortLinkOfUser = await _unitOfWork.ShortLinks.GetListShortLinkIDOfUser(userId);
            if (listTokenShortLinkOfUser.Count > 0)
            {
                foreach (var item in listTokenShortLinkOfUser)
                {
					var count = await _unitOfWork.ViewDetails.CountClickByDateRange(start, end, item);
					countClick += count;
                }
            }
            //count click thanh cong
            return Ok(countClick);
        }

    }
}
