using AutoMapper;
using Classifieds.Domain.Model;
using Classifieds.Service;
using Classifieds.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Threading.Tasks;

namespace Classifieds.Web.Controllers
{
    public class MenuController : Controller
    {
        private IMenuService menuService;
        private IMapper mapper;

        public MenuController(IMenuService menuService, IMapper mapper)
        {
            this.menuService = menuService;
            this.mapper = mapper;
        }
        /// <summary>
        /// /Menu/SubMenus/2
        /// Fetch all submenus with parentId {id}
        /// </summary>
        /// <param name="id">parent id</param>
        /// <returns></returns>
        public IActionResult SubMenus(int id)
        {
            Expression<Func<Menu, bool>> whereCondition = m => m.ParentID == id;

            var subMenus = menuService.FindAll(whereCondition, null) as List<Menu>;

            return Ok(subMenus);

        }
        /// <summary>
        /// Create a new menu item
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public IActionResult Create()
        {
           

            ViewBag.Menus = MenuSelectListItems(-1);
            return PartialView();
        }
        /// <summary>
        /// Create menu submit method
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public IActionResult Create(MenuViewModel model)
        {
            if (ModelState.IsValid)
            {
                model.Label = model.Name;

                if(model.ParentID > 0)
                {
                    MenuViewModel parent = mapper.Map<MenuViewModel>
                        (menuService.Find(model.ParentID.Value));
                    model.Parent = parent;
                }
                menuService.Create(mapper.Map<Menu>(model));
                menuService.Save();

                //Update session variables
                HttpContext.Session.SetString("SideMenus", JsonConvert.SerializeObject(GetMenus(),
              Formatting.Indented, new JsonSerializerSettings
              {
                  ReferenceLoopHandling = ReferenceLoopHandling.Ignore
              }));

                HttpContext.Response.StatusCode = (int)HttpStatusCode.Created;
                return new JsonResult("Menu Created!");
            }

            ViewBag.Menus = MenuSelectListItems(model.ParentID);
            HttpContext.Response.StatusCode = (int)HttpStatusCode.OK;
            return PartialView(model);
        }
        /// <summary>
        /// Update checkbox for admin 
        /// </summary>
        /// <param name="id">ID of menu</param>
        /// <param name="isAdmin">checkbox true/false</param>
        /// <returns></returns>
        [Authorize]
        public IActionResult Admin(long id, bool isAdmin)
        {
            Menu menu = menuService.Find(id);

            if (isAdmin)
            {
                menu.Admin = 1;

            }
            else
            {
                menu.Admin = 0;
            }

            menuService.Update(menu);
            menuService.Save();

            return new JsonResult("Saved!");

        }
        /// <summary>
        /// Get menu list using predicate
        /// </summary>
        /// <returns></returns>
        private IEnumerable<MenuViewModel> GetMenus()
        {
            Expression<Func<Menu, bool>> where = m => m.ParentID == null;
            Expression<Func<Menu, object>>[] include =
            {
                m => m.SubMenus
            };

            IEnumerable<MenuViewModel> menus = mapper.Map<IEnumerable<MenuViewModel>>
                    (menuService.FindAll(where, include));

            return menus;
        }
        /// <summary>
        /// Get SelectList items to popolate dropdown list
        /// </summary>
        /// <param name="selectedItem"></param>
        /// <returns></returns>
        private List<SelectListItem> MenuSelectListItems(long? selectedItem)
        {
            Expression<Func<Menu, bool>> whereCondition = m => m.ParentID == null;
            var menus = menuService.FindAll(whereCondition, null) as List<Menu>;

            List<SelectListItem> selectList = new List<SelectListItem>();

            foreach(var menu in menus)
            {
                selectList.Add(new SelectListItem
                {
                    Text = menu.Name,
                    Value = menu.ID.ToString(),
                    Selected = menu.ID == selectedItem? true:false
                });
            }

            return selectList;
        }
    }
}
