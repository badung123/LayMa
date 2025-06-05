using Azure.Core;
using LayMa.Core.ConfigOptions;
using LayMa.Core.Domain.Identity;
using LayMa.Core.Domain.Link;
using LayMa.Core.Domain.Transaction;
using LayMa.Core.Interface;
using LayMa.Core.Model.Auth;
using LayMa.Core.Model.Campain;
using LayMa.Core.Model.CodeManager;
using LayMa.Core.Utilities;
using LayMa.Data.Migrations;
using LayMa.WebAPI.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Org.BouncyCastle.Tsp;

namespace LayMa.WebAPI.Controllers.AdminApi
{
	[Route("api/admin/codemanager")]
	[ApiController]
	public class CodeManagerController : ControllerBase
	{
		private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<AppUser> _userManager;
        private readonly WhiteListIPGetCode _whiteListIP;
        public CodeManagerController(IUnitOfWork unitOfWork, UserManager<AppUser> userManager, IOptions<WhiteListIPGetCode> whiteListIP)
		{
			_unitOfWork = unitOfWork;
			_userManager = userManager;
			_whiteListIP = whiteListIP.Value;
		}

		[HttpPost]
		[Route("createcode")]
		public async Task<ActionResult<dynamic>> CreateCode([FromBody] InsertCodeRequest request)
		{
			if (request == null)
			{
				return BadRequest("Invalid request");
			}
			var Id = Guid.NewGuid();
			//get keysearhID
			var keysearhID = await _unitOfWork.KeySearchs.GetKeySearchIDByKey(request.Key);
			if (keysearhID == null)
			{
				return BadRequest("Key Search không hợp lệ");
			}
			var code = new Code
			{
				Id = Id,
				CodeString = request.Code,
				DateCreated = DateTime.Now,
				DateModified = DateTime.Now,
				IsUsed = false,
				KeySearchId = keysearhID.Value
			};
			return Ok();
		}

		[HttpPost]
		[Route("checkcode")]
		public async Task<ActionResult<dynamic>> CheckCode([FromBody] CheckCodeRequest request)
		{
			if (request == null)
			{
				return BadRequest("Invalid request");
			}
			string ip = HttpContext.GetServerVariable("REMOTE_HOST");
			if (ip == null)
			{
				ip = this.HttpContext.GetServerVariable("REMOTE_ADDR");
			}
			var ips = HttpContext.Request.GetIpAddress();
			var shortLink = await _unitOfWork.ShortLinks.GetByTokenAsync(request.Token);
			
			if (shortLink == null) return BadRequest("Link rút gọn " + request.Token + " không tồn tại");
			var userId = shortLink.UserId;
			var user = await _userManager.FindByIdAsync(userId.ToString());
			if (user == null || user.IsActive == false) return BadRequest("User không hợp lệ");
			var date = DateTime.Now;
			var start = date.Date;
			var end = date.Date.AddDays(1);
			if (user.MaxClickInDay > 0)
			{
				var countClickInDay = await _unitOfWork.ViewDetails.CountClickByDateRangeAndUserId(start, end, user.Id);
				if (countClickInDay >= user.MaxClickInDay) return BadRequest("Đã hết lượt view trong ngày");
			}
            //tiến hành update          
            var campain = await _unitOfWork.Campains.GetCampainByCampainID(Guid.Parse(request.CampainId));
			if (campain == null) return BadRequest("Chiến dịch không hợp lệ hoặc đã tạm dừng");
			//dua vao tu khoa de lay ra campainId
			bool check = false;
			if (campain.Flatform == "google")
			{
				check = await _unitOfWork.CodeManagers.CheckCodeGoogle(request.Code, campain.KeyToken);
			}
			else
			{
				check = await _unitOfWork.CodeManagers.CheckCode(request.Code, campain.Id);
			}
			
			if (!check) return BadRequest("Code " + request.Code + " đã được dùng hoặc không hợp lệ");

			if (campain.TypeRun == 1)
			{
				start = start.AddHours(date.Hour);
				end = start.AddHours(1);
			}
			var countCampain = await _unitOfWork.ViewDetails.CountClickByDateRangeAndCampainId(start, end, campain.Id);
			if (campain.TypeRun == 0)
            {
                if (campain.Status)
                {
					if (campain.ViewPerDay < countCampain) return Ok(shortLink.Duphong);
					if (countCampain == (campain.ViewPerDay - 1))
					{
						//tat chien dich
						await _unitOfWork.Campains.UpdateActive(campain.Id, false);
					}
				}           
			}
			if (campain.TypeRun == 1)			
			{
                if (campain.Status && countCampain == (campain.ViewPerHour - 1))
                {
					await _unitOfWork.Campains.UpdateActive(campain.Id, false);
				}
            }
			//check 1 ngày 1 user chỉ đc dùng 1 IP,user agent
			var checkIp = await _unitOfWork.ViewDetails.CheckIP(ips, request.DeviceScreen);
			if (!checkIp) {
				var transLog = new TransactionLog
				{
					Id = Guid.NewGuid(),
					UserId = user.Id,
					UserName = user.UserName,
					Amount = 0,
					OldBalance = Int64.Parse(user.Balance.ToString()),
					CreatedBy = user.UserName,
					Description = "trùng IP - " + shortLink.Link,
					TranSactionType = TranSactionType.ClickCode,
					ShortLink = shortLink.Link,
					DeviceScreen = request.DeviceScreen,
					UserAgent = request.UserAgent,
					IPAddress = ips,
					DateCreated = DateTime.Now,
					DateModified = DateTime.Now,
					Flatform = campain.Flatform
				};
				_unitOfWork.TransactionLogs.Add(transLog);
				await _unitOfWork.CompleteAsync();
				return Ok(shortLink.OriginLink);
			}
			//update code đã dùng
			int? solution = null;
			if (campain.Flatform == "google")
			{
				solution = await _unitOfWork.CodeManagers.UpdateIsUsedGoogle(request.Code, campain.KeyToken);
			}
			else
			{
				solution = await _unitOfWork.CodeManagers.UpdateIsUsed(request.Code, campain.Id);
			}
			if (solution == null) return BadRequest("Không xác định được bạn đang chạy theo phương pháp nào của nhà cung cấp.Vui lòng liên hệ nhà cung cấp");
			//insert view detail
			var viewDetail = new ViewDetail()
			{
				Id = Guid.NewGuid(),
				Device = "",
				ShortLinkId = shortLink.Id,
				UserId = shortLink.UserId,
				DateCreated = DateTime.Now,
				DateModified = DateTime.Now,
				DeviceScreen = request.DeviceScreen,
				UserAgent = request.UserAgent,
				IPAddress = ips,
				CampainId = campain.Id,
				Solution = solution
			};
			_unitOfWork.ViewDetails.Add(viewDetail);

			await _unitOfWork.Users.UpdateBalanceCount(user.Id, user.Rate);
			//get start of shortlink
			var startTimeShortLink = await _unitOfWork.Visitors.GetStartTimeOfShortLink(user.Id, shortLink.Id);

			var timeDiff = DateTime.Now.Subtract(startTimeShortLink).TotalSeconds;
			//TODO: update vào bảng transation
			var transLogUser = new TransactionLog
			{
				Id = Guid.NewGuid(),
				UserId = user.Id,
				UserName = user.UserName,
				Amount = user.Rate,
				OldBalance = Int64.Parse(user.Balance.ToString()),
				CreatedBy = user.UserName,
				Description = "Nhận thưởng nhập code thành công link rút gọn : " + shortLink.Link,
				TranSactionType = TranSactionType.ClickCode,
				ShortLink = shortLink.Link,
				DeviceScreen = request.DeviceScreen,
				UserAgent = request.UserAgent,
				IPAddress = ips,
				DateCreated = DateTime.Now,
				DateModified = DateTime.Now,
				Flatform = campain.Flatform,
				Solution = solution,
				TimeFinish = (int)timeDiff
			};
			_unitOfWork.TransactionLogs.Add(transLogUser);
			if (!string.IsNullOrEmpty(user.Agent))
			{
				var agent = await _unitOfWork.Users.GetUserAgentByRefcode(user.Agent);
				if (agent != null)
				{
					await _unitOfWork.Users.UpdateBalanceCount(agent.Id, 60);
					//TODO: update vào bảng transation
					var transLogAgent = new TransactionLog
					{
						Id = Guid.NewGuid(),
						UserId = agent.Id,
						UserName = agent.UserName,
						Amount = 60,
						OldBalance = Int64.Parse(agent.Balance.ToString()),
						CreatedBy = user.UserName,
						ShortLink = shortLink.Link,
						Description = "Nhận thưởng hoa hồng thành công link rút gọn : " + shortLink.Link + " của tài khoản " + user.UserName,
						TranSactionType = TranSactionType.Commission,
						DateCreated = DateTime.Now,
						DateModified = DateTime.Now,
                        Flatform = campain.Flatform
                    };
					_unitOfWork.TransactionLogs.Add(transLogAgent);
				}
			}
			var result = await _unitOfWork.CompleteAsync();
			return result > 0 ? Ok(shortLink.OriginLink) : BadRequest("Nhập code không thành công");

		}

		[HttpPost]
		[Route("getcode")]
		public async Task<ActionResult<string>> GetCode([FromBody] Dictionary<string, dynamic> request)
		{
            // bo sung cac thong tin ip,browser,client ui sau
            //Dictionary<string, string> res = JsonConvert.DeserializeObject<Dictionary<string, string>>(request);
   //         var ips = HttpContext.Request.GetIpAddress();
   //         var listIP = _whiteListIP.WhiteList.Split(',').ToList();
			//if (!listIP.Contains(ips)) return BadRequest(new CodeResponse { Success = false,Html=ips });
            var token = "";
			token = token.GenerateLinkToken(6);
			var Id = Guid.NewGuid();
			var code = new Code
			{
				Id = Id,
				CodeString = token,
				DateCreated = DateTime.Now,
				DateModified = DateTime.Now,
				IsUsed = false,
				KeySearchId = Guid.NewGuid(),
				CampainId = Guid.Parse(request["trafficid"].ToString()),
				IPAddress = "",
				Solution = Int32.Parse(request["solution"].ToString()) + 1
			};
			_unitOfWork.CodeManagers.Add(code);
			var result = await _unitOfWork.CompleteAsync();			
			return result > 0 ? Ok(new CodeResponse { Success = true,Html = token}) : BadRequest(new CodeResponse { Success = false});
		}
	}
}
