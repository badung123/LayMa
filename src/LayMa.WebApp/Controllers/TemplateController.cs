using LayMa.WebApp.Constant;
using LayMa.WebApp.Extensions;
using LayMa.WebApp.Interface;
using LayMa.WebApp.Models;
using LayMa.WebApp.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Options;
using Microsoft.Extensions.Options;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace LayMa.WebApp.Controllers
{
    public class TemplateController : Controller
    {
        private readonly ILogger<TemplateController> _logger;
		private readonly IConfiguration _configuration;
		private readonly GoogleCaptchaOptions _googleCaptchaOptions; 
		private readonly ICaptchaService _captchaService;
        private readonly IRecaptchaExtension _recaptcha;
        public TemplateController(ILogger<TemplateController> logger, IConfiguration configuration, IOptions<GoogleCaptchaOptions> googleCaptchaOptions, ICaptchaService captchaService, IRecaptchaExtension recaptcha)
        {
            _logger = logger;
			_configuration = configuration;
            _googleCaptchaOptions = googleCaptchaOptions.Value;
            _captchaService = captchaService;
            _recaptcha = recaptcha;
        }
        [HttpGet("/{id}")]
        public async Task<IActionResult> Index(string id)
        {
			//updateView
			//cứ vào là tính 1 lượt view
            ViewBag.Id = id;
			var url = _configuration.GetValue<string>("API_URL");
			ViewBag.ApiUrl = url;
			string apiUrl = url + "/api/admin/mission?token=" + id; //https://api.layma.net,https://localhost:7020
            var siteKey = _configuration.GetSection("GoogleRecaptcha").GetSection("Sitekey").Value ?? "";
			ViewBag.SiteKey = siteKey;

			var table = new TemplateViewModel();
			using (HttpClient client = new HttpClient())
			{
                Console.WriteLine("Da vao");
                client.BaseAddress = new Uri(apiUrl);
				client.DefaultRequestHeaders.Accept.Clear();
				client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

				HttpResponseMessage response = await client.GetAsync(apiUrl);
				
				if (response.IsSuccessStatusCode)
				{
					var data = await response.Content.ReadAsStringAsync();
					Console.WriteLine(data);
					table = Newtonsoft.Json.JsonConvert.DeserializeObject<TemplateViewModel>(data);
					if (!string.IsNullOrEmpty(table.UrlImage)) table.UrlImage = url + table.UrlImage;

				}
			}
			//call api get model by token id
			//var templateModel = new TemplateViewModel { Key = "fb88",UrlImage = "https://cdn.24h.com.vn/upload/4-2024/images/2024-10-17/8-740-1729174343-73-width740height416.jpg" };
			//thống kê chuyển qua link dự phòng
			if (table.IsHetMa && !string.IsNullOrEmpty(table.LinkDuPhong)) return Redirect(table.LinkDuPhong);
			return View(table);
        }

        [HttpGet]
        public async Task<JsonResult> Verify(string token)
        {
            var verified = await _recaptcha.VerifyAsync(token);

            return Json(verified);
        }
        public IActionResult Test(User user)
        {
            if (ModelState.IsValid && UserService.IsValid(user))
            {
                return RedirectToAction("Welcome", "Home");
            }
            ViewBag.Captcha = new CaptchaViewModel
            {
                SiteKey = _googleCaptchaOptions.SiteKey,
                IsEnabled = _googleCaptchaOptions.Enabled,
                Action = _googleCaptchaOptions.Action,
                Version = _googleCaptchaOptions.Version
            };
            return View();
        }
    }
}
