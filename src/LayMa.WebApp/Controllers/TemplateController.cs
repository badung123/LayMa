using LayMa.WebApp.Models;
using Microsoft.AspNetCore.Mvc;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace LayMa.WebApp.Controllers
{
    public class TemplateController : Controller
    {
        private readonly ILogger<TemplateController> _logger;
		private IConfiguration _configuration;
		public TemplateController(ILogger<TemplateController> logger, IConfiguration configuration)
        {
            _logger = logger;
			_configuration = configuration;
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
            return View(table);
        }
    }
}
