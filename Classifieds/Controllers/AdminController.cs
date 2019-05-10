using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Web;
using System.Threading.Tasks;
using AutoMapper;
using Classifieds.Domain.Model;
using Classifieds.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using Classifieds.Web.Models;
using System.Linq;

namespace Classifieds.Web.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        private IMenuService menuService;
        private IMapper mapper;

        public AdminController(IMenuService menuService, IMapper mapper)
        {
            this.menuService = menuService;
            this.mapper = mapper;
        }
        /// <summary>
        /// Admin home page
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            Expression<Func<Menu, bool>> where = m => m.ParentID == null;
            Expression<Func<Menu, object>>[] include =
            {
                m => m.SubMenus
            };

            IEnumerable<MenuViewModel> menus = mapper.Map<IEnumerable<MenuViewModel>>
                    (menuService.FindAll(where, include));

            HttpContext.Session.SetString("SideMenus", JsonConvert.SerializeObject(menus,
                Formatting.Indented, new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                }));

            return View();
        }
        
    }
}