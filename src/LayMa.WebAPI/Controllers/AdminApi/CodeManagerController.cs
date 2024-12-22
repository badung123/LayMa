using Azure.Core;
using LayMa.Core.Domain.Identity;
using LayMa.Core.Domain.Link;
using LayMa.Core.Domain.Transaction;
using LayMa.Core.Interface;
using LayMa.Core.Model.Auth;
using LayMa.Core.Model.CodeManager;
using LayMa.Core.Utilities;
using LayMa.Data.Migrations;
using LayMa.WebAPI.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace LayMa.WebAPI.Controllers.AdminApi
{
	[Route("api/admin/codemanager")]
	[ApiController]
	public class CodeManagerController : ControllerBase
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly UserManager<AppUser> _userManager;
		public CodeManagerController(IUnitOfWork unitOfWork, UserManager<AppUser> userManager)
		{
			_unitOfWork = unitOfWork;
			_userManager = userManager;
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
			//var keysearhID = await _unitOfWork.KeySearchs.GetKeySearchIDByKey(request.Key);
			//if (keysearhID == null)
			//{
			//	return BadRequest("Key Search không hợp lệ");
			//}
			var check = await _unitOfWork.CodeManagers.CheckCode(request.Code, Guid.Parse(request.CampainId));
            if (!check)
            {
				return BadRequest("Code đã được dùng hoặc không hợp lệ");
			}
			//tiến hành update
			//get UserID by token key
			var shortLink = await _unitOfWork.ShortLinks.GetByTokenAsync(request.Token);
            if (shortLink == null)
            {
				return BadRequest("Link rút gọn không tồn tại");
			}
            var userId = shortLink.UserId;
			var user = await _userManager.FindByIdAsync(userId.ToString());
			if (user == null || user.IsActive == false)
			{
				return BadRequest("User không hợp lệ");
			}	
			// phải nằm trong transaction
			//update viewcount in shortlink
			await _unitOfWork.ShortLinks.UpdateViewCount(shortLink.Id);
			//update code đã dùng
			await _unitOfWork.CodeManagers.UpdateIsUsed(request.Code, Guid.Parse(request.CampainId));

			//insert view detail
			var viewDetail = new ViewDetail()
			{
				Id = Guid.NewGuid(),
				Device = ips,
				IPAddress = ip,
				ShortLinkId = shortLink.Id,
				DateCreated = DateTime.Now,
				DateModified = DateTime.Now
			};
			_unitOfWork.ViewDetails.Add(viewDetail);
			//get User and update balane
			await _unitOfWork.Users.UpdateBalanceCount(user.Id,1000);
			//TODO: update vào bảng transation
			var transLogUser = new TransactionLog
			{
				Id = Guid.NewGuid(),
				UserId = user.Id,
				UserName = user.UserName,
				Amount = 1000,
				OldBalance = Int64.Parse(user.Balance.ToString()),
				CreatedBy = user.UserName,
				Description = "Nhận thưởng nhập code thành công link rút gọn : " + shortLink.Link,
				TranSactionType = TranSactionType.ClickCode,
				DateCreated = DateTime.Now,
				DateModified = DateTime.Now
			};
			_unitOfWork.TransactionLogs.Add(transLogUser);
			if (!string.IsNullOrEmpty(user.Agent))
			{
				var agent = await _unitOfWork.Users.GetUserAgentByRefcode(user.Agent);
				if (agent != null)
				{
					await _unitOfWork.Users.UpdateBalanceCount(agent.Id, 100);
					//TODO: update vào bảng transation
					var transLogAgent = new TransactionLog
					{
						Id = Guid.NewGuid(),
						UserId = agent.Id,
						UserName = agent.UserName,
						Amount = 100,
						OldBalance = Int64.Parse(user.Balance.ToString()),
						CreatedBy = agent.UserName,
						Description = "Nhận thưởng hoa hồng thành công link rút gọn : " + shortLink.Link + " của tài khoản " + user.UserName,
						TranSactionType = TranSactionType.Commission,
						DateCreated = DateTime.Now,
						DateModified = DateTime.Now
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
				CampainId = Guid.Parse(request["trafficid"].ToString())
			};
			_unitOfWork.CodeManagers.Add(code);
			var result = await _unitOfWork.CompleteAsync();			
			return result > 0 ? Ok(new CodeResponse { Success = true,Html = token}) : BadRequest(new CodeResponse { Success = false});
		}
	}
}
