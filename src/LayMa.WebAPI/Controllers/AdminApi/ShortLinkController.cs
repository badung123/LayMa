using AutoMapper;
using LayMa.Core.Domain.Identity;
using LayMa.Core.Domain.Link;
using LayMa.Core.Domain.Mission;
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
			var token = "";
			token = token.GenerateLinkToken(9);
			link.Token = token;
			link.Link = "https://layma.net/" + token; //https://localhost:7181/,https://layma.net/
			_unitOfWork.ShortLinks.Add(link);
			//add nhiem vu
			//get campainid random
			var campainId = await _unitOfWork.Campains.GetCampainIdRandom();
			if (campainId == Guid.Empty) return BadRequest();
			var missionId = Guid.NewGuid();
			var mission = new Mission() { 
				Id = missionId,
				CampainId = campainId,
				ShortLinkId = id,
				TokenUrl = token,
				ShortLink = request.Url,
				UserId = userId,
				DateCreated = DateTime.Now,
				DateModified = DateTime.Now,
				IsActive = true
			};
			_unitOfWork.Missions.Add(mission);
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
        public async Task<ActionResult<ThongKeViewClick>> GetThongKeClickByDate(DateTime from,DateTime to)
        {
			var countClick = 0;
			var countView = 0;
			var userId = User.GetUserId();
			//get list token short link
			var listTokenShortLinkOfUser = await _unitOfWork.ShortLinks.GetListShortLinkIDOfUser(userId);
            if (listTokenShortLinkOfUser.Count > 0)
            {
                foreach (var item in listTokenShortLinkOfUser)
                {
					var countC = await _unitOfWork.ViewDetails.CountClickByDateRangeAndShortLink(from, to, item);
					countClick += countC;
					var countV = await _unitOfWork.Visitors.CountViewByDateRangeAndShortLink(from, to, item);
					countView += countV;

				}
            }
            //count click thanh cong
            return Ok(new ThongKeViewClick
			{
				Click = countClick,
				View = countView
			});
        }
        [HttpGet]
        [Route("thongkeAllClickInDay")]
        public async Task<ActionResult<int>> thongkeAllClickInDay()
        {
            var date = DateTime.Now;
            var start = date.Date;
            var end = date.Date.AddDays(1);
            //get list token short link
            var count = await _unitOfWork.ViewDetails.CountClickByDateRange(start, end);
            //count click thanh cong
            return Ok(count);
        }
        [HttpPost]
        [Route("updateNguon")]
        public async Task<ActionResult> UpdateNguon([FromBody] UpdateNguon request)
        {
            //get list token short link
            await _unitOfWork.ShortLinks.UpdateOriginOfShortLink(request.Origin, request.ShortlinkId,request.Duphong);
			await _unitOfWork.CompleteAsync();
            //count click thanh cong
            return Ok();
        }
		[HttpGet]
		[Route("gethoahongbydate")]
		public async Task<ActionResult<int>> GetHoaHongByDate(DateTime from,DateTime to)
		{
			var userId = User.GetUserId();
			if (userId == Guid.Empty) return BadRequest("User hết phiên làm việc");
			var hoahong = await _unitOfWork.TransactionLogs.GetHoaHongByDate(userId,from,to);
			return Ok(hoahong);
		}
		[HttpGet]
		[Route("paging-log-shortlink")]
		//[Authorize(ShortLinks.View)]
		public async Task<ActionResult<PagedResult<LogShortLinkDto>>> GetLogShortLinkPaging(DateTime from, DateTime to, int pageIndex = 1, int pageSize = 10, string? userName = "", int type = -1, string? userAgent = "", string? shortLink = "", string? screen = "", string? ip = "", string? flatform = "")
		{
			var result = await _unitOfWork.ShortLinks.GetAllLogPaging(from,to,pageIndex, pageSize, userName,type,userAgent,shortLink,screen,ip,flatform);
			return Ok(result);
		}
	}
}
