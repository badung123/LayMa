using LayMa.WebApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace LayMa.WebApp.Controllers
{
	public class TrafficController : Controller
	{
		private IConfiguration _configuration;
		public TrafficController(IConfiguration configuration)
		{
			_configuration = configuration;
		}
		[HttpGet]
		[Route("Traffic/Index/{id?}")]
		public async Task<IActionResult> Index(string id)
		{
			ViewBag.Id = id;
			var url = _configuration.GetValue<string>("API_URL");
			ViewBag.ApiUrl = url;
			
			// Get hCaptcha site key from configuration
			var hCaptchaSiteKey = _configuration.GetValue<string>("HCaptcha:SiteKey") ?? "your-hcaptcha-site-key";
			ViewBag.HCaptchaSiteKey = hCaptchaSiteKey;
			
			//var apiUrl = url + "/api/admin/campain?keytoken=" + id;
			////get campain or keysearch by token id
			////string apiUrl = "https://api.layma.net/api/admin/campain?keytoken=" + id; //https://api.layma.net,https://localhost:7020
			//var table = new CampainViewModel();
			//using (HttpClient client = new HttpClient())
			//{
			//	client.BaseAddress = new Uri(apiUrl);
			//	client.DefaultRequestHeaders.Accept.Clear();
			//	client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

			//	HttpResponseMessage response = await client.GetAsync(apiUrl);

			//	if (response.IsSuccessStatusCode)
			//	{
			//		var data = await response.Content.ReadAsStringAsync();
			//		Console.WriteLine(data);
			//		table = Newtonsoft.Json.JsonConvert.DeserializeObject<CampainViewModel>(data);
			//	}
			//}
			return View();
		}
	}
}
