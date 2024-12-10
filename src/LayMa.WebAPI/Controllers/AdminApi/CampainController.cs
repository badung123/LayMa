using LayMa.Core.Interface;
using LayMa.Core.Model.Campain;
using LayMa.Core.Model.KeySearch;
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
		[HttpGet]
		public async Task<ActionResult<CampainDto?>> GetCampainByKeyToken(string keytoken)
		{
			var result = await _unitOfWork.Campains.GetCampainByKeyToken(keytoken);
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
    }
}
