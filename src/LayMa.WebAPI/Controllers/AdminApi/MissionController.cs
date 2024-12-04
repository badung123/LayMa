using Azure.Core;
using LayMa.Core.Interface;
using LayMa.Core.Model.Mission;
using Microsoft.AspNetCore.Mvc;

namespace LayMa.WebAPI.Controllers.AdminApi
{
	[Route("api/admin/mission")]
	[ApiController]
	public class MissionController : ControllerBase
	{
		private readonly IUnitOfWork _unitOfWork;
		public MissionController(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}
		[HttpGet]
		public async Task<ActionResult<MissionDto>> GetMissionByTokenShortLink(string token)
		{
			var shortLink = await _unitOfWork.ShortLinks.GetByTokenAsync(token);
			if (shortLink == null)
			{
				return BadRequest("Link rút gọn không tồn tại");
			}
			var userId = shortLink.UserId;
			//get mission by userid
			var mission = await _unitOfWork.Missions.GetMissionByUserId(userId);
			if (mission == null) return BadRequest("Không tìm thấy nhiệm vụ nào cho bạn");
			//get flatform
			var campain = await _unitOfWork.Campains.GetByIdAsync(mission.CampainId);
			if (campain == null) return BadRequest("Không có chiến dịch phù hợp");
			var missionDto = new MissionDto()
			{
				Id = mission.Id,
				Key	= campain.KeySearch,
				UrlImage = campain.ImageUrl,
				UrlVideo = campain.VideoUrl,
				Flatfrom = campain.Flatform,
				UrlFacebook = campain.Url,
				UrlWeb = campain.Url,
				CampainId = campain.Id,
			};
			return Ok(missionDto);
		}
	}
}
