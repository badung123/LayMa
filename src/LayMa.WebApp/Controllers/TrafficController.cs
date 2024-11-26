using LayMa.WebApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace LayMa.WebApp.Controllers
{
	public class TrafficController : Controller
	{
		[HttpGet]
		[Route("Traffic/Index/{id?}")]
		public async Task<IActionResult> Index(string id)
		{
			ViewBag.Id = id;
			//get campain or keysearch by token id
			string apiUrl = "https://localhost:7020/api/admin/campain?keytoken=" + id; //https://api.layma.net,https://localhost:7020
			var table = new CampainViewModel();
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
					table = Newtonsoft.Json.JsonConvert.DeserializeObject<CampainViewModel>(data);
				}
			}
			return View(table);
		}
	}
}
