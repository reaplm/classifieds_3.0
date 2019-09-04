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
using System.Linq.Expressions;

namespace Classifieds.Controllers
{
    public class HomeController : Controller
    {
        private ICategoryService categoryService;
        private IMapper mapper;

        public HomeController(ICategoryService categoryService, IMapper mapper)
        {
            this.categoryService = categoryService;
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

            Expression<Func<Category, object>>[] include =
            {
                c => c.SubCategories
            };
            Expression<Func<Category, bool>> where = c => c.ParentID == null;

            IEnumerable<CategoryViewModel> categories = mapper.Map<IEnumerable<CategoryViewModel>>
                (categoryService.FindAll(where, include));

            return View(categories); 
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
            return View();
        }
    }
}
