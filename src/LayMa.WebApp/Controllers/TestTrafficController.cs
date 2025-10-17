using LayMa.WebApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace LayMa.WebApp.Controllers
{
    public class TestTrafficController : Controller
	{
		public TestTrafficController()
		{
			
		}
		[HttpGet]
		[Route("TestTraffic/Index/{id?}")]
		public async Task<IActionResult> Index(string id)
		{
			ViewBag.Id = id;
			var url = "";
			ViewBag.ApiUrl = url;

			// Get hCaptcha site key from configuration
			var hCaptchaSiteKey = "";
			ViewBag.HCaptchaSiteKey = hCaptchaSiteKey;
			var hCaptchaTokenDefault = "";
			ViewBag.HCaptchaTokenDefault = hCaptchaTokenDefault;
			
			return View();
		}
	}
}
