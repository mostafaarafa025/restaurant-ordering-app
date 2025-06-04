using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantApp.Areas.Admin.Repositories;
using RestaurantApp.Models;

namespace RestaurantApp.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        IItemRepository ItemRepo;
        public HomeController(IItemRepository _Repo, ILogger<HomeController> logger)
        {
            _logger = logger;
            ItemRepo = _Repo;
        }

        public IActionResult Index()
        {
            var items = ItemRepo.GetItems().Where(i => i.IsTodaySpecial);
            return View(items);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
