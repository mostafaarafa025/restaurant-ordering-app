﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantApp.Areas.Admin.Models;
using RestaurantApp.Areas.Admin.Repositories;
using RestaurantApp.Data;

namespace RestaurantApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    
    public class CategoryController : Controller
    {
        //RestaurantDbContext context;
        ICategoryRepository Repo;
        private IWebHostEnvironment webHostEnvironment;
        public CategoryController(ICategoryRepository _Repo, IWebHostEnvironment _webHostEnvironment)
        {
            Repo = _Repo;
            webHostEnvironment = _webHostEnvironment;

        }

        public IActionResult Index()
        {
            var model = Repo.GetAll();
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
            if (!ModelState.IsValid)
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
            Repo.Add(category);

            return RedirectToAction("index");
        }

        public IActionResult Edit(int id)
        {
            var category = Repo.GetById(id);
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
            var category = Repo.GetById(model.Id);
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
            Repo.Update(category);
            return RedirectToAction("Index");
        }

        public IActionResult delete(int id)
        {
            var category = Repo.GetById(id);
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
            Repo.Delete(category);
            return RedirectToAction("index");
        }
    }
}
