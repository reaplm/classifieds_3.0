using AutoMapper;
using Classifieds.Domain.Enumerated;
using Classifieds.Domain.Model;
using Classifieds.Service;
using Classifieds.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Classifieds.Web.Controllers
{
    public class ClassifiedsController : Controller
    {
        private IAdvertService advertService;
        private ICategoryService categoryService;
        private IMapper mapper;

        public ClassifiedsController(IAdvertService advertService, 
            ICategoryService categoryService,
            IMapper mapper)
        {
            this.advertService = advertService;
            this.categoryService = categoryService;
            this.mapper = mapper;
        }
        /// <summary>
        /// /Classifieds home
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            Expression<Func<Advert, object>>[] include =
            {
                a => a.Detail
            };
            var adverts = advertService.FindAll(null, include);

            return View(adverts);
        }
        /// <summary>
        /// Displays adverts belonging to the category of the given id
        /// url: Classifieds/Category/id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IActionResult Category(int id)
        {
            var adverts = advertService.FindByCategory(id);
            return View(adverts);
        }
        /// <summary>
        /// Creates an advert. 
        /// User must log in inorder to post an advert
        /// url: Classifieds/create
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public IActionResult Create()
        {
            var model = new AdvertViewModel();

            var userId = HttpContext.User.Claims.FirstOrDefault(u => u.Type == "UserId").Value;
            model.UserID = long.Parse(userId);


            //Initially there's no selected item
            ViewBag.Categories = CatSelectListItems(-1);
            ViewBag.SubCategories = new List<SelectListItem>();//empty initially

            return View(model);
        }
        /// <summary>
        /// Create new advert
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public IActionResult Create(AdvertViewModel model)
        {
            if (ModelState.IsValid)
            {
                model.SubmittedDate = DateTime.Now;
                model.Status = EnumTypes.AdvertStatus.SUBMITTED.ToString();

                advertService.Create(mapper.Map<Advert>(model));
                advertService.Save();
                return RedirectToAction("Index", "Classifieds");
            }

            ViewBag.Categories = CatSelectListItems(model.ParentID);
            ViewBag.SubCategories = SubCatSelectListItems(model.ParentID, model.CategoryID);

            return View(model);
        }
        /// <summary>
        /// Return advert details
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IActionResult Detail(long id)
        {
            Expression<Func<Advert, object>>[] include =
            {
                a => a.Detail,
                a => a.Detail.AdPictures,
                a => a.Category,
                a => a.User
            };
            AdvertViewModel model = mapper.Map<AdvertViewModel>(advertService.Find(id, include));

            return PartialView(model);
        }
        /// <summary>
        /// Fetch parent categories
        /// </summary>
        /// <param name="selectedVal">The ID of the selected item</param>
        /// <returns>List of SelectListItem</returns>
        private IEnumerable<SelectListItem> CatSelectListItems(int? selectedVal)
        {
            Expression<Func<Category, bool>> where = c => c.ParentID == null;

            IEnumerable<Category> categories = categoryService.FindAll(where, null);
            List<SelectListItem> selectItems = new List<SelectListItem>();

            foreach (var category in categories)
            {
                selectItems.Add(new SelectListItem
                {
                    Text = category.Name,
                    Value = category.ID.ToString(),
                    Selected = category.ID == selectedVal ? true : false
                });
            }

            return selectItems;
        }
        /// <summary>
        /// Get submenus for a given parent menu
        /// </summary>
        /// <param name="parentId">The ID of the parent entity</param>
        /// <param name="selectedVal">The ID of the selected item</param>
        /// <returns>SelectListItem</returns>
        private IEnumerable<SelectListItem> SubCatSelectListItems(long parentId, long? selectedVal)
        {
            Expression<Func<Category, bool>> where = x => x.ParentID == parentId;

            IEnumerable<Category> categories = categoryService.FindAll(where, null);
            List<SelectListItem> selectItems = new List<SelectListItem>();

            foreach (var category in categories)
            {
                selectItems.Add(new SelectListItem
                {
                    Text = category.Name,
                    Value = category.ID.ToString(),
                    Selected = category.ID == selectedVal ? true : false
                });
            }

            return selectItems;
        }

    }
}
