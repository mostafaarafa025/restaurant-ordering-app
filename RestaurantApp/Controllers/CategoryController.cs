using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantApp.Data;
using RestaurantApp.Models;

namespace RestaurantApp.Controllers
{
    public class CategoryController : Controller
    {
        RestaurantDbContext context;
        private  IWebHostEnvironment webHostEnvironment;
        public CategoryController(RestaurantDbContext _context , IWebHostEnvironment _webHostEnvironment) {
        this.context = _context;
        this.webHostEnvironment = _webHostEnvironment;
            
        }

        public IActionResult Index()
        {
           var model= context.Categories.ToList();
            return View(model);
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CategoryViewModel model)
        {
            if(!ModelState.IsValid)
            return View(model);
            string imagePath = null;
            if (model.ImageFile != null)
            {
                var uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "images/categories");
                Directory.CreateDirectory(uploadsFolder); 
                var uniqueFileName = Guid.NewGuid().ToString() + "_" + model.ImageFile.FileName;
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await model.ImageFile.CopyToAsync(stream);
                }

                imagePath = "/images/categories/" + uniqueFileName;
            }
            var category = new Category()
            {
                Name = model.Name,
                Description = model.Description,
                ImagePath = imagePath
            };
            context.Categories.Add(category);
           await context.SaveChangesAsync();

                return RedirectToAction("index");
        }

        public IActionResult Edit(int id)
        {
           var category= context.Categories.Include(i=>i.Items).FirstOrDefault(i=>i.Id==id);
            CategoryViewModel model = new CategoryViewModel()
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description,
                ExistingImagePath = category.ImagePath
            };
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CategoryViewModel model)
        {
            if (!ModelState.IsValid) 
                return View(model);
            var category = context.Categories.Find(model.Id);
            if (category == null)
                return NotFound();
            category.Name = model.Name;
            category.Description = model.Description;
            if (model.ImageFile != null)
            {
                var uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "images/categories");
                Directory.CreateDirectory(uploadsFolder);
                var uniqueFileName = Guid.NewGuid().ToString() + "_" + model.ImageFile.FileName;
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await model.ImageFile.CopyToAsync(stream);
                }
                if (!string.IsNullOrEmpty(model.ExistingImagePath))
                {
                    var oldImagePath = Path.Combine(webHostEnvironment.WebRootPath, model.ExistingImagePath.TrimStart('/'));
                    if (System.IO.File.Exists(oldImagePath))
                        System.IO.File.Delete(oldImagePath);
                }

                category.ImagePath = "/images/categories/" + uniqueFileName;
            }
            context.Categories.Update(category);
            await context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> delete(int id)
        {
            var category = context.Categories.Find(id);
            if (category == null)
                return NotFound();
            if (!string.IsNullOrEmpty(category.ImagePath))
            {
                var imagePath = Path.Combine(webHostEnvironment.WebRootPath, category.ImagePath.TrimStart('/'));
                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }
            }
            context.Categories.Remove(category);
           await context.SaveChangesAsync();
            return RedirectToAction("index");
        }
    }
}
