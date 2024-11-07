using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LayMa.WebAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[Authorize]
	public class TestController : ControllerBase
	{
		[HttpGet]
		public IActionResult TestAuthen()
		{
			return Ok();
		}
	}
}
