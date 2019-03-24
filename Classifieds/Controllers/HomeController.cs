using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Classifieds.Models;
using Classifieds.Service;

namespace Classifieds.Controllers
{
    public class HomeController : Controller
    {
        private IMenuService menuService;

        public HomeController(IMenuService menuService)
        {
            this.menuService = menuService;
        }

        public IActionResult Index()
        {
           
            ViewBag.Title = "Adpost Home";
            ViewBag.Menus = menuService.findAll();

            return View();
            
        }

        public IActionResult Classifieds()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Login()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
