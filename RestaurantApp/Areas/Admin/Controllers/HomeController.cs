﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace RestaurantApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        // Add other admin controller actions here
        public IActionResult Dashboard()
        {
            return View();
        }
    }
}