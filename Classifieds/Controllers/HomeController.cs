using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Classifieds.Models;
using Classifieds.Service;
using Classifieds.Domain.Model;
using AutoMapper;
using Classifieds.Web.Models;

namespace Classifieds.Controllers
{
    public class HomeController : Controller
    {
        private IMenuService menuService;
        private IMapper mapper;

        public HomeController(IMenuService menuService, IMapper mapper)
        {
            this.menuService = menuService;
            this.mapper = mapper;
        }
        /// <summary>
        /// Home Page
        /// /Home
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
           
            ViewBag.Title = "Adpost Home";
            IEnumerable<MenuViewModel> menus = mapper.Map<IEnumerable<MenuViewModel>>
                (menuService.FindByType(new String[] { "HOME" }));

            return View(menus);
            
        }
        /// <summary>
        /// Classifieds link on header menu
        /// /Classifieds
        /// </summary>
        public void Classifieds()
        {
            RedirectToAction("Index", "Classifieds");
        }
        /// <summary>
        /// Login link on header menu
        /// /Login
        /// </summary>
        /// <returns></returns>
        public IActionResult Login()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }
        /// <summary>
        /// Error page
        /// </summary>
        /// <returns></returns>
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
