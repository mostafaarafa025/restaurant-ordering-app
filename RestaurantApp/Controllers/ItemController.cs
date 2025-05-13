using Microsoft.AspNetCore.Mvc;
using RestaurantApp.Data;
using RestaurantApp.Models;
using RestaurantApp.Repositories;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;

namespace RestaurantApp.Controllers
{
    public class ItemController : Controller
    {
        // RestaurantDbContext context;
        IItemRepository ItemRepo;
        IWebHostEnvironment webHostEnvironment;
        ICategoryRepository CateRepo;
        public ItemController(IItemRepository _Repo, IWebHostEnvironment _webHostEnvironment,ICategoryRepository _CatRepo)
        {
        this.ItemRepo = _Repo;
        this.webHostEnvironment = _webHostEnvironment;
            this.CateRepo = _CatRepo;
        }
        public IActionResult Index()
        {
           var categories= CateRepo.GetAll();
            ViewBag.categories = categories;
            var items = ItemRepo.GetItems();

            return View(items);
        }

        public IActionResult create()
        {
            ViewBag.categories = CateRepo.GetAll();
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ItemViewModel model)
        {
            if(!ModelState.IsValid)
                return View(model);
            string imagePath = null;

            if (model.ImageFile != null)
            {
                var uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "images/items");
                Directory.CreateDirectory(uploadsFolder);

                var uniqueFileName = Guid.NewGuid().ToString() + "_" + model.ImageFile.FileName;
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var image = await SixLabors.ImageSharp.Image.LoadAsync(model.ImageFile.OpenReadStream()))
                {
                    image.Mutate(x => x.Resize(new ResizeOptions
                    {
                        Mode = ResizeMode.Max,
                        Size = new Size(800, 800) 
                    }));

                    var encoder = new JpegEncoder
                    {
                        Quality = 75 
                    };

                    await image.SaveAsync(filePath, encoder);
                }

                imagePath = "/images/items/" + uniqueFileName;
            }

            Item item = new Item()
            {
                Name = model.Name,
                Description = model.Description,
                Price = model.Price,
                Calories = model.Calories,
                CategoryId = model.CategoryId,
                ImagePath = imagePath,
                IsAvailable = model.IsAvailable,
                Ingredients = model.Ingredients,
            };
            ItemRepo.Add(item);
            return RedirectToAction("index");
        }

        public IActionResult Edit(int id)
        {
            var item = ItemRepo.getById(id);
            if (item == null) 
                return NotFound();
            var model = new ItemViewModel()
            {
                Id = item.Id,
                Name = item.Name,
                Description = item.Description,
                Price = item.Price,
                Calories = item.Calories,
                IsAvailable= item.IsAvailable,
                Ingredients=item.Ingredients,
                CategoryId = item.CategoryId,
                ExistingImagePath = item.ImagePath,
            };
            var categories = CateRepo.GetAll();
            ViewBag.categories = categories;
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ItemViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.categories = CateRepo.GetAll();
                return View(model);
            }
            string imagePath = model.ExistingImagePath;

            if (model.ImageFile != null)
            {
                var uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "images/items");
                Directory.CreateDirectory(uploadsFolder);

                var uniqueFileName = Guid.NewGuid().ToString() + "_" + model.ImageFile.FileName;
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var image = await SixLabors.ImageSharp.Image.LoadAsync(model.ImageFile.OpenReadStream()))
                {
                    image.Mutate(x => x.Resize(new ResizeOptions
                    {
                        Mode = ResizeMode.Max,
                        Size = new Size(800, 800)
                    }));

                    var encoder = new JpegEncoder
                    {
                        Quality = 75
                    };

                    await image.SaveAsync(filePath, encoder);
                }

                imagePath = "/images/items/" + uniqueFileName;
                if (!string.IsNullOrEmpty(model.ExistingImagePath))
                {
                    var oldImagePath = Path.Combine(webHostEnvironment.WebRootPath, model.ExistingImagePath.TrimStart('/'));
                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }
            }
            var item =ItemRepo.getById(model.Id);
            item.Name = model.Name;
            item.Description = model.Description;
            item.Price = model.Price;
            item.Calories = model.Calories;
            item.CategoryId = model.CategoryId;
            item.ImagePath = imagePath;  
            item.IsAvailable = model.IsAvailable;
            item.Ingredients = model.Ingredients;

            ItemRepo.Update(item);

            return RedirectToAction("index");
        } 
        public IActionResult delete(int? id)
        {
            if (id == null)
                return BadRequest();
            var model = ItemRepo.getById(id.Value);
             ItemRepo.Delete(model);
            return RedirectToAction("index");
        }
    }
}
