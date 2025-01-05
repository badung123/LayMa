using LayMa.WebApp.Extensions;
using LayMa.WebApp.Models;
using Microsoft.AspNetCore.Mvc;
using System.Configuration;
using System.Diagnostics;

namespace LayMa.WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
		private IConfiguration _configuration;
       

        public HomeController(ILogger<HomeController> logger, IConfiguration configuration)
        {
            _logger = logger;
			_configuration = configuration;
		}

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult DieuKhoan()
        {
            return View();
        }
        public IActionResult MucTraThuong()
        {
            return View();
        }
		public IActionResult DaiLy(string refcode)
		{
			var manager_url = _configuration.GetValue<string>("Manager_URL");
			if (!string.IsNullOrEmpty(refcode))
            {
				return Redirect(manager_url + "/auth/register?refcode=" + refcode);
			}
			return Redirect(manager_url + "/auth/register");
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        

    }
}
