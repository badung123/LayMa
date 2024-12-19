using LayMa.Core.Domain.Link;
using LayMa.Core.Domain.Visitor;
using LayMa.Core.Interface;
using LayMa.Core.Model.CodeManager;
using LayMa.WebAPI.Extensions;
using Microsoft.AspNetCore.Mvc;
using YamlDotNet.Core.Tokens;

namespace LayMa.WebAPI.Controllers.AdminApi
{
	[Route("api/admin/visitor")]
	[ApiController]
	public class VisitorController : ControllerBase
	{
		private readonly IUnitOfWork _unitOfWork;
		public VisitorController(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}
		[HttpPost]
		public async Task<ActionResult<string>> SaveVisitor([FromBody] Dictionary<string, dynamic> request)
		{
			// bo sung cac thong tin ip,browser,client ui sau
			//Dictionary<string, string> res = JsonConvert.DeserializeObject<Dictionary<string, string>>(request);
			//
			var token = request["token"].ToString();
			var shortLink = await _unitOfWork.ShortLinks.GetByTokenAsync(token);
			if (shortLink == null)
			{
				return BadRequest("Link rút gọn không tồn tại");
			}
			var shortlinkID = shortLink.Id;
			var userID = shortLink.UserId;
			var Id = Guid.NewGuid();
			var visitor = new Visitor
			{
				Id = Id,
				DateCreated = DateTime.Now,
				DateModified = DateTime.Now,
				VisitorId = request["uuid"].ToString(),
				Browser = request["browser"].ToString(),
				BrowserVersion = request["browserVersion"].ToString(),
				BrowserMajorVersion = request["browserMajorVersion"].ToString(),
				Cookies = Boolean.Parse(request["cookies"].ToString()),
				OS = request["os"].ToString(),
				OSVersion = request["osVersion"].ToString(),
				Mobile = Boolean.Parse(request["mobile"].ToString()),
				DeviceScreen = request["screen"].ToString(),
				ShortLinkId = Guid.Parse(shortlinkID.ToString()),
				UserId = Guid.Parse(userID.ToString()),
				UserAgent = request["userAgent"].ToString()
			};
			_unitOfWork.Visitors.Add(visitor);
			var result = await _unitOfWork.CompleteAsync();
			return result > 0 ? Ok(new CodeResponse { Success = true }) : BadRequest(new CodeResponse { Success = false });
		}
	}
}
