using AutoMapper;
using LayMa.Core.Domain.Campain;
using LayMa.Core.Domain.Identity;
using LayMa.Core.Domain.Link;
using LayMa.Core.Domain.Mission;
using LayMa.Core.Interface;
using LayMa.Core.Model;
using LayMa.Core.Model.Campain;
using LayMa.Core.Model.CodeManager;
using LayMa.Core.Model.ShortLink;
using LayMa.WebAPI.Extensions;
using LayMa.WebAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System.Reflection.Metadata.Ecma335;
using static LayMa.Core.Constants.AdminPermissions;
using static LayMa.Core.Constants.Permissions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
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
		public ShortLinkController(IUnitOfWork unitOfWork, UserManager<AppUser> userManager, IMapper mapper, IShortLinkService shortLinkService)
		{
			_mapper = mapper;
			_unitOfWork = unitOfWork;
			_userManager = userManager;
			_shortLinkService = shortLinkService;
		}
		[HttpPost]
		[Route("create")]
		//[Authorize(ShortLinks.Create)]
		public async Task<IActionResult> CreateShortLink([FromBody] CreateShortLinkDto request)
		{
			var userId = User.GetUserId();
			var user = await _userManager.FindByIdAsync(userId.ToString());
			var link = _mapper.Map<CreateShortLinkDto, ShortLink>(request);
			link.DateCreated = DateTime.Now;
			link.DateModified = DateTime.Now;
			link.UserId = userId;
			var id = Guid.NewGuid();
			link.Id = id;
			link.OriginLink = request.Url;
			var token = "";
			token = token.GenerateLinkToken(9);
			link.Token = token;
			link.From = "Web";
			link.Link = "https://layma.net/" + token; //https://localhost:7181/,https://layma.net/
			_unitOfWork.ShortLinks.Add(link);
			//add nhiem vu
			//get campainid random
			//check xem có campain nào đang tắt và view = 0 thì bật lên
			var listAutoOff = await _unitOfWork.Campains.GetListCampainAutoOff();
			
			if (listAutoOff.Count > 0) {
                foreach (var autoOff in listAutoOff)
                {
					var date = DateTime.Now;
					var start = date.Date;
					var end = date.Date.AddDays(1);
					if (autoOff.TypeRun == 1)
					{
						start = start.AddHours(date.Hour);
						end = start.AddHours(1);
					}
					//check count
					var countCampain = await _unitOfWork.ViewDetails.CountClickByDateRangeAndCampainId(start, end, autoOff.Id);
                    if (countCampain == 0) await _unitOfWork.Campains.UpdateActive(autoOff.Id, true);
				}
            }
			var campainId = await _unitOfWork.Campains.GetCampainIdRandom();
			if (campainId == Guid.Empty) return BadRequest();
			var missionId = Guid.NewGuid();
			var mission = new Mission()
			{
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
		public async Task<ActionResult<PagedResult<ShortLinkInListDto>>> GetPostsPaging(int pageIndex, int pageSize = 10, string? keySearch = "")
		{
			var userId = User.GetUserId();
			var result = await _unitOfWork.ShortLinks.GetAllPaging(userId, pageIndex, pageSize, keySearch);
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
		public async Task<ActionResult<ThongKeViewClick>> GetThongKeClickByDate(DateTime from, DateTime to)
		{
			from = from.ToLocalTime().Date;
			to = to.ToLocalTime().Date;
			var countClick = 0;
			var countView = 0;
			var userId = User.GetUserId();
			//get list token short link
			//var listTokenShortLinkOfUser = await _unitOfWork.ShortLinks.GetListShortLinkIDOfUser(userId);
			//if (listTokenShortLinkOfUser.Count > 0)
			//{
			//	foreach (var item in listTokenShortLinkOfUser)
			//	{
			//		var countC = await _unitOfWork.ViewDetails.CountClickByDateRangeAndShortLink(from, to, item);
			//		countClick += countC;
			//	}
			//}
			countClick = await _unitOfWork.ViewDetails.CountClickByDateRangeAndUserId(from, to, userId);

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
			await _unitOfWork.ShortLinks.UpdateOriginOfShortLink(request.Origin, request.ShortlinkId, request.Duphong);
			await _unitOfWork.CompleteAsync();
			//count click thanh cong
			return Ok();
		}
		[HttpGet]
		[Route("gethoahongbydate")]
		public async Task<ActionResult<int>> GetHoaHongByDate(DateTime from, DateTime to)
		{
			from = from.ToLocalTime().Date;
			to = to.ToLocalTime().Date;
			var userId = User.GetUserId();
			if (userId == Guid.Empty) return BadRequest("User hết phiên làm việc");
			var hoahong = await _unitOfWork.TransactionLogs.GetHoaHongByDate(userId, from, to);
			return Ok(hoahong);
		}
		[HttpGet]
		[Route("paging-log-shortlink")]
		//[Authorize(ShortLinks.View)]
		public async Task<ActionResult<PagedResult<LogShortLinkDto>>> GetLogShortLinkPaging(DateTime from, DateTime to, int pageIndex = 1, int pageSize = 10, string? userName = "", int type = -1, string? userAgent = "", string? shortLink = "", string? screen = "", string? ip = "", string? flatform = "",int? solution = 0)
		{
			from = from.ToLocalTime().Date;
			to = to.ToLocalTime().Date;
			var result = await _unitOfWork.ShortLinks.GetAllLogPaging(from, to, pageIndex, pageSize, userName, type, userAgent, shortLink, screen, ip, flatform,solution);
			return Ok(result);
		}
		[HttpGet]
		[Route("quicklink")]
		//[Authorize(ShortLinks.Create)]
		public async Task<ActionResult<dynamic>> CreateShortLinkByQickLink(string tokenUser, string? format, string url,string ? link_du_phong)
		{
			var user = await _unitOfWork.Users.GetUserByUserToken(tokenUser);
			var token = "";
			token = token.GenerateLinkToken(9);
			var link = new ShortLink
			{
				Id = Guid.NewGuid(),
				OriginLink = url,
				Link = "https://layma.net/" + token,
				Token = token,
				DateCreated = DateTime.Now,
				DateModified = DateTime.Now,
				Duphong = string.IsNullOrEmpty(link_du_phong) ? "" : link_du_phong,
				Origin = "",
				UserId = user.Id,
				View = 0,
				ViewCount = 0,
				From = "API"
			};
			_unitOfWork.ShortLinks.Add(link);

			var listAutoOff = await _unitOfWork.Campains.GetListCampainAutoOff();

			if (listAutoOff.Count > 0)
			{
				foreach (var autoOff in listAutoOff)
				{
					var date = DateTime.Now;
					var start = date.Date;
					var end = date.Date.AddDays(1);
					if (autoOff.TypeRun == 1)
					{
						start = start.AddHours(date.Hour);
						end = start.AddHours(1);
					}
					//check count
					var countCampain = await _unitOfWork.ViewDetails.CountClickByDateRangeAndCampainId(start, end, autoOff.Id);
					if (countCampain == 0) await _unitOfWork.Campains.UpdateActive(autoOff.Id, true);
				}
			}
			//add nhiem vu
			//get campainid random
			var campainId = await _unitOfWork.Campains.GetCampainIdRandom();
			if (campainId == Guid.Empty) return BadRequest();
			var missionId = Guid.NewGuid();
			var mission = new Mission()
			{
				Id = missionId,
				CampainId = campainId,
				ShortLinkId = link.Id,
				TokenUrl = token,
				ShortLink = url,
				UserId = user.Id,
				DateCreated = DateTime.Now,
				DateModified = DateTime.Now,
				IsActive = true
			};
			_unitOfWork.Missions.Add(mission);
			var result = await _unitOfWork.CompleteAsync();
			if (format != null && format != "")
			{
				if (format.ToLower() == "text")
				{
					return result > 0 ? Ok(link.Link) : BadRequest(new CodeResponse { Success = false, Html = "" });
				}
				if (format.ToLower() == "json")
				{
					return result > 0 ? Ok(new CodeResponse { Success = true, Html = link.Link }) : BadRequest(new CodeResponse { Success = false, Html = "" });
				}
			}
			return result > 0 ? Redirect(link.Link) : BadRequest(new CodeResponse { Success = false, Html = "" });
		}
		[HttpGet]
		[Route("getApiUserToken")]
		//[Authorize(ShortLinks.Create)]
		public async Task<ActionResult<string>> GetApiUserToken()
		{
			var userId = User.GetUserId();
			var user = await _userManager.FindByIdAsync(userId.ToString());
			if (user == null) return BadRequest("User đã hết phiên đăng nhập");
			return Ok(new CodeResponse { Success = true, Html = user.ApiUserToken });
		}
		//[HttpPost]
		//[Route("lockShortLink")]
		//public async Task<ActionResult> LockShortLink(LockShortLinkRequest request)
		//{
		//	//var userId = User.GetUserId();
		//	if (request == null) return BadRequest("Lỗi đầu vào");
		//	await _unitOfWork.Campains.UpdateActive(request.Id, request.IsActive);
		//	await _unitOfWork.CompleteAsync();
		//	//change mission
		//	return Ok();
		//}
		[HttpGet]
		[Route("thongkeClickUserByDate")]
		public async Task<ActionResult<PagedResult<ThongKeViewClickByUser>>> GetThongKeClickUserByDate(DateTime from, DateTime to, int pageIndex = 1, int pageSize = 10, string? userName = "")
		{
			from = from.ToLocalTime().Date;
			to = to.ToLocalTime().Date;
			//get list User
			var listUserThongKe = await _unitOfWork.Users.GetAllUser(userName);
			//var newList = new List<ThongKeViewClickByUser>();
			 

			foreach (var user in listUserThongKe)
			{
				var countCLick = 0;
				if (userName != "")
				{
					countCLick = await _unitOfWork.ViewDetails.CountClickByDateRangeAndUserId(from, to, user.Id);
				}
				else
				{
					countCLick = await _unitOfWork.ViewDetails.CountClickByDateRangeAndUserIdOld(from, to, user.Id);
				}
				
                if (countCLick > 0)
				{
					user.Click = countCLick;
					user.View = await _unitOfWork.Visitors.CountViewByDateRangeAndUserId(from, to, user.Id);
				}

			}
			var query = listUserThongKe.AsQueryable().Where(x=> x.View > 0 && x.Click > 0);
			var totalRow = query.Count();
			var listPaging = query.OrderByDescending(x => x.Click)
			   .Skip((pageIndex - 1) * pageSize)
			   .Take(pageSize);
			//count click thanh cong
			var rs = new PagedResult<ThongKeViewClickByUser>
			{
				Results = listPaging.ToList(),
				CurrentPage = pageIndex,
				RowCount = totalRow,
				PageSize = pageSize
			};
			return rs;
		}
		[HttpGet]
		[Route("thongkeClickViewUserInMonth")]
		public async Task<ThongKeClickViewUser30Day> thongkeClickViewUserInMonth(Guid userId)
		{
			var rs = new ThongKeClickViewUser30Day();
			var listDate = new List<string>();
			var lstClick = new List<int>();
			var lstView = new List<int>();
			var dateNow = DateTime.Now.Date;
			var dateBefere30 = dateNow.AddDays(-30);
			var listClick = await _unitOfWork.ViewDetails.CountClickByDateUserIdInMonth(userId);
			var listView = await _unitOfWork.Visitors.CountViewByDateUserIdInMonth(userId);
			while (dateBefere30 <= dateNow) {
				
				var click = listClick.Where(x => x.Date == dateBefere30).FirstOrDefault();
				var countClick = click != null ? click.Count : 0;
				lstClick.Add(countClick);
				var view = listView.Where(x => x.Date == dateBefere30).FirstOrDefault();
				var countView = view != null ? view.Count : 0;
				lstView.Add(countView);
				listDate.Add(dateBefere30.ToString("dd-MM"));
				dateBefere30 = dateBefere30.AddDays(1);
			}
			rs.Date = listDate;
			rs.Clicks = lstClick;
			rs.Views = lstView;
			return rs;
		}

        [HttpGet]
        [Route("testThongkeClickByDateAndUser")]
        public async Task<ActionResult<int>> GetTestThongKeClickByDateAndUser(DateTime from, DateTime to,Guid userId)
        {
            from = from.ToLocalTime().Date;
            to = to.ToLocalTime().Date;
            var countClick = await _unitOfWork.ViewDetails.CountClickByDateRangeAndUserId(from, to, userId);

            //count click thanh cong
            return countClick;
        }
    }
}
