using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WorkCalendarV2.Models;

namespace WorkCalendarV2.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return Json("Go to Activity/GetAllActivitiesPerEmployee");
        }

        public IActionResult Privacy()
        {
            return View();
        }

    }
}
