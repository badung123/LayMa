using LayMa.Core.Domain.Identity;
using LayMa.Core.Domain.Link;
using LayMa.Core.Interface;
using LayMa.Core.Model.Auth;
using LayMa.Core.Model.CodeManager;
using LayMa.Core.Utilities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

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
            var keysearhID = await _unitOfWork.KeySearchs.GetKeySearchIDByKey(request.Key);
            if (keysearhID == null)
            {
				return BadRequest("Key Search không hợp lệ");
			}
            var check = await _unitOfWork.CodeManagers.CheckCode(request.Code, keysearhID.Value);
            if (!check)
            {
				return BadRequest("Invalid request");
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
			if (user == null || user.IsActive == false || user.LockoutEnabled)
			{
				return BadRequest("User không hợp lệ");
			}
			//get User and update balane
			user.Balance += 500;
			await _userManager.UpdateAsync(user);
			//update viewcount in shortlink
			await _unitOfWork.ShortLinks.UpdateViewCount(shortLink.Id);
			//update code đã dùng
			await _unitOfWork.CodeManagers.UpdateIsUsed(request.Code, keysearhID.Value);
            
            //insert view detail
            var viewDetail = new ViewDetail()
			{
				Id = Guid.NewGuid(),
				Device = "",
				IPAddress = ips,
				ShortLinkId = shortLink.Id,
				DateCreated = DateTime.Now,
				DateModified = DateTime.Now
			};
			_unitOfWork.ViewDetails.Add(viewDetail);
			
			var result = await _unitOfWork.CompleteAsync();
			return result > 0 ? Ok() : BadRequest();

		}

		[HttpGet]
		[Route("getcode")]
		public async Task<ActionResult<string>> GetCode(string trafficid)
		{
			
			return Ok("12345");
		}
	}
}
