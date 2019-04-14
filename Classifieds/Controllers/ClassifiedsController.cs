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
using System.Threading.Tasks;

namespace Classifieds.Web.Controllers
{
    

     
    public class ClassifiedsController : Controller
    {
        private IAdvertService advertService;
        private IMenuService menuService;
        private IMapper mapper;

        public ClassifiedsController(IAdvertService advertService, IMenuService menuService,
            IMapper mapper)
        {
            this.advertService = advertService;
            this.menuService = menuService;
            this.mapper = mapper;
        }
        /// <summary>
        /// /Classifieds home
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            var adverts = advertService.findAll();

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
            var adverts = advertService.findByCategory(id);
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
                ViewBag.Menus = MenuSelectListItems(-1);
                ViewBag.SubMenus = new List<SelectListItem>();//empty initially

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

                advertService.create(mapper.Map<Advert>(model));
                advertService.save();
                return RedirectToAction("Index", "Classifieds");
            }

            ViewBag.Menus = MenuSelectListItems(model.ParentID);
            ViewBag.SubMenus = SubMenuSelectListItems(model.ParentID, model.MenuID);

            return View(model);
        }
        /// <summary>
        /// Return advert details
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IActionResult Detail(long id)
        {
            AdvertViewModel model = mapper.Map<AdvertViewModel>(advertService.Find(id));

            return PartialView(model);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="selectedVal">The ID of the selected item</param>
        /// <returns>List of SelectListItem</returns>
        private IEnumerable<SelectListItem> MenuSelectListItems(int? selectedVal)
        {
            IEnumerable<Menu> menus = menuService.findByType(new String[] { "HOME" });
            List<SelectListItem> selectItems = new List<SelectListItem>();

            foreach(var menu in menus)
            {
                selectItems.Add(new SelectListItem
                {
                    Text = menu.Name,
                    Value = menu.ID.ToString(),
                    Selected = menu.ID == selectedVal ? true : false
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
        private IEnumerable<SelectListItem> SubMenuSelectListItems(long parentId, long? selectedVal)
        {
            IEnumerable<Menu> menus = menuService.findAll(parentId);
            List<SelectListItem> selectItems = new List<SelectListItem>();

            foreach (var menu in menus)
            {
                selectItems.Add(new SelectListItem
                {
                    Text = menu.Name,
                    Value = menu.ID.ToString(),
                    Selected = menu.ID == selectedVal ? true : false
                });
            }

            return selectItems;
        }

    }
}
