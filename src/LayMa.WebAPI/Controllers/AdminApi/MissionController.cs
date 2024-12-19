using Azure.Core;
using LayMa.Core.Domain.Campain;
using LayMa.Core.Domain.Mission;
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
			//shortLink.View += 1;
			
			//await _unitOfWork.ShortLinks.UpdateView(shortLink.Id);
            var userId = shortLink.UserId;
			//get mission by userid
			var mission = await _unitOfWork.Missions.GetMissionByUserId(userId,shortLink.Id);
			if (mission == null) return BadRequest("Không tìm thấy nhiệm vụ nào cho bạn");
			//get flatform
			var campain = await _unitOfWork.Campains.GetCampainByID(mission.CampainId);
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
		[HttpPost]
		[Route("changeMission")]
		public async Task<ActionResult> ChangeMission([FromBody] ChangeMissionRequest request)
		{
			var shortLink = await _unitOfWork.ShortLinks.GetByTokenAsync(request.Token);
			if (shortLink == null)
			{
				return BadRequest("Link rút gọn không tồn tại");
			}
			var userId = shortLink.UserId;
			//get mission by userid
			var mission = await _unitOfWork.Missions.GetMissionByUserId(userId, shortLink.Id);
			if (mission == null) return BadRequest("Không tìm thấy nhiệm vụ nào cho bạn");
			//update mission isactive = false
			await _unitOfWork.Missions.UpdateIsChange(mission.Id);
			var campainId = await _unitOfWork.Campains.GetCampainIdRandomByOldID(mission.CampainId);
			if (campainId == Guid.Empty) return BadRequest("Hiện tại không có chiến dịch nào phù hợp");
			var newMissionId = Guid.NewGuid();
			var newMission = new Mission()
			{
				Id = newMissionId,
				CampainId = campainId,
				ShortLinkId = shortLink.Id,
				TokenUrl = request.Token,
				ShortLink = shortLink.OriginLink,
				UserId = userId,
				DateCreated = DateTime.Now,
				DateModified = DateTime.Now,
				IsActive = true
			};
			_unitOfWork.Missions.Add(newMission);
			var result = await _unitOfWork.CompleteAsync();
			return Ok();
		}
	}
}
