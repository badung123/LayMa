using LayMa.Core.Domain.Link;
using LayMa.Core.Interface;
using LayMa.Core.Model.KeySearch;
using Microsoft.AspNetCore.Mvc;
using static LayMa.Core.Constants.Permissions;

namespace LayMa.WebAPI.Controllers.AdminApi
{
	[Route("api/admin/keyseo")]
	[ApiController]
	public class KeySearchController : ControllerBase
	{
		private readonly IUnitOfWork _unitOfWork;
		public KeySearchController(IUnitOfWork unitOfWork) {
			_unitOfWork = unitOfWork;
		}
		[HttpGet]
		public async Task<ActionResult<KeySeoDto?>> GetInfoKeySeoRandom()
		{
			var result = await _unitOfWork.KeySearchs.GetInfoKeySeo();
			return Ok(result);
		}
		[HttpGet]
		[Route("thongkeview")]
		public async Task<ActionResult<ThongKeView>> GetThongKeView()
		{
            var result = await _unitOfWork.KeySearchs.GetThongKeView();
            return Ok(result);
        }

    }
}
