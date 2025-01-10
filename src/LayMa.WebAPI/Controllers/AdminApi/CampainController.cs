using LayMa.Core.Domain.Campain;
using LayMa.Core.Interface;
using LayMa.Core.Model;
using LayMa.Core.Model.Campain;
using LayMa.Core.Model.KeySearch;
using LayMa.Core.Model.ShortLink;
using LayMa.WebAPI.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace LayMa.WebAPI.Controllers.AdminApi
{
	[Route("api/admin/campain")]
	[ApiController]
	public class CampainController : ControllerBase
	{
		private readonly IUnitOfWork _unitOfWork;
		public CampainController(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}
		[HttpPost]
		public async Task<ActionResult> CreateOrUpdateCampain(CreateOrUpdateCampainRequest request)
		{
			if (request == null) return BadRequest("Lỗi đầu vào");

			if (request.CampainId == Guid.Empty)
			{
				var token = "";
				token = token.GenerateLinkToken(9);
				var campainCreate = new Campain
				{
					Id = Guid.NewGuid(),
					Flatform = request.Flatform,
					DateCreated = DateTime.Now,
					DateModified = DateTime.Now,
					KeyToken = token,
					Decription = "",
					Status = true,
					TimeOnSitePerView = request.Time,
					Url = request.UrlWeb,
					ImageUrl = request.Thumbnail,
					KeySearch = request.Key,
					PricePerView = request.Price,
					RemainView = 0,
					ToTalPrice = 0,
					ToTalView = 0,
					VideoUrl = "",
					ViewPerDay = request.View					
				};
				_unitOfWork.Campains.Add(campainCreate);
			}
			else
			{
				var campainUpdate = await _unitOfWork.Campains.GetCampainByIDNotCheckStatus(request.CampainId);
				campainUpdate.PricePerView = request.Price;
				campainUpdate.KeySearch = request.Key;
				campainUpdate.ViewPerDay = request.View;
				campainUpdate.KeySearch = request.Key;
				campainUpdate.ImageUrl = request.Thumbnail;
				campainUpdate.TimeOnSitePerView= request.Time;
				campainUpdate.DateModified = DateTime.Now;
				_unitOfWork.Campains.Update(campainUpdate);
			}
			var result = await _unitOfWork.CompleteAsync();
			return result > 0 ? Ok() : BadRequest();
		} 
		[HttpGet]
		public async Task<ActionResult<CampainDto?>> GetCampainByKeyToken(string keytoken)
		{
			var result = await _unitOfWork.Campains.GetCampainByKeyToken(keytoken);
			return Ok(result);
		}
		[HttpGet]
		[Route("getCampainByCampainId")]
		public async Task<ActionResult<CampainInListDto?>> GetCampainByCampainId(Guid campainId)
		{
			var result = await _unitOfWork.Campains.GetCampainByCampainID(campainId);
			return Ok(result);
		}
		[HttpGet]
        [Route("thongkeview")]
        public async Task<ActionResult<ThongKeView>> GetThongKeView()
        {
            var result = await _unitOfWork.Campains.GetThongKeView();
			if (result != null) {
                var date = DateTime.Now;
                var start = date.Date;
                var end = date.Date.AddDays(1);
                result.ViewedInDay = await _unitOfWork.ViewDetails.CountClickByDateRange(start,end);
			}
			
            return Ok(result);
        }
		[HttpGet]
		[Route("paging")]
		public async Task<ActionResult<PagedResult<CampainInListDto>>> GetPostsPaging(int pageIndex, int pageSize = 10,string flatform = "google", string? keySearch = "")
		{
			//var userId = User.GetUserId();
			var date = DateTime.Now;
			var start = date.Date;
			var end = date.Date.AddDays(1);
			var result = await _unitOfWork.Campains.GetAllPaging(pageIndex, pageSize, flatform, keySearch);
			foreach (var item in result.Results) {		
				var viewCountInday = await _unitOfWork.ViewDetails.CountClickByDateRangeAndCampainId(start, end, item.Id);
				item.ToTalView = viewCountInday;
			}
			return Ok(result);
		}
        [HttpPost]
        [Route("turnoffCampain")]
        public async Task<ActionResult> TurnOffOrOnCampain(TurnOffOrOnCampainRequest request)
        {
            //var userId = User.GetUserId();
            if (request == null) return BadRequest("Lỗi đầu vào");
			await _unitOfWork.Campains.UpdateActive(request.Id, request.IsActive);
			await _unitOfWork.CompleteAsync();
			//change mission
			return Ok();
        }

    }
}
