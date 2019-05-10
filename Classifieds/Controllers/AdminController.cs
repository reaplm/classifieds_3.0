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
        IUserService userService;

        private IMapper mapper;

        public AdminController(IMenuService menuService, IUserService userService,
            IMapper mapper)
        {
            this.menuService = menuService;
            this.userService = userService;
            this.mapper = mapper;
        }
        /// <summary>
        /// Admin Dashboard Page
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
        public IActionResult Profile()
        {
            var userId = HttpContext.User.Claims.FirstOrDefault(u => u.Type == "UserId").Value;
            UserViewModel model = null;
            Expression<Func<User, object>>[] include =
            {
                u => u.UserDetail
            };

            if (userId != null)
            {
                model = mapper.Map<UserViewModel>(userService.Find(long.Parse(userId), include));
            }
            
            return View(model);

        }
    }
}