using Microsoft.AspNetCore.Mvc;
using RestaurantApp.Areas.Admin.Repositories;

namespace RestaurantApp.Controllers
{
    public class MenuController : Controller
    {
        IItemRepository ItemRepo;
        ICategoryRepository CateRepo;
        public MenuController(IItemRepository _Repo, ICategoryRepository _CatRepo)
        {
            ItemRepo = _Repo;
            CateRepo = _CatRepo;
        }
        public IActionResult Index()
        {
            
            ViewBag.Categories = CateRepo.GetAll().ToList();
            ViewBag.Items = ItemRepo.GetItems().ToList();
            return View();
        }
    }
}
