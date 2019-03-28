using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Classifieds.Models;
using Classifieds.Service;
using Classifieds.Domain.Model;

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
            IEnumerable<Menu> Menus = menuService.findByType(new String[] { "HOME" }) as IEnumerable<Menu>;

            return View(Menus);
            
        }

        public void Classifieds()
        {
            RedirectToAction("Index", "Classifieds");
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
