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
        private IUserService userService;
        private IAdvertService advertService;
        private IMapper mapper;

        public AdminController(IMenuService menuService, IUserService userService,
            IAdvertService advertService, IMapper mapper)
        {
            this.menuService = menuService;
            this.userService = userService;
            this.advertService = advertService;
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
                u => u.UserDetail,
                u => u.UserDetail.Address
            };

            if (userId != null)
            {
                model = mapper.Map<UserViewModel>(userService.Find(long.Parse(userId), include));
            }
            
            return View(model);

        }
        /// <summary>
        /// /Admin/Menus
        /// </summary>
        /// <returns></returns>
        public IActionResult Menus()
        {
            Expression<Func<Menu, bool>> where = m => m.ParentID == null;

            Expression<Func<Menu, object>>[] include =
            {
                m => m.SubMenus
            };

            var menus = mapper.Map<IEnumerable<MenuViewModel>>(menuService.FindAll(where, include));

            return View(menus);
        }
        /// <summary>
        /// /Admin/Adverts
        /// </summary>
        /// <returns></returns>
        public IActionResult Adverts()
        {
            Expression<Func<Advert, object>>[] include =
            {
                m => m.Detail
            };

            var adverts = mapper.Map<IEnumerable<AdvertViewModel>>
                (advertService.FindAll(null, include));

            return View(adverts);
        }
        /// <summary>
        /// /Admin/Users
        /// </summary>
        /// <returns></returns>
        public IActionResult Users()
        {
            Expression<Func<User, object>>[] include =
            {
                m => m.UserDetail
            };

            var users= mapper.Map<IEnumerable<UserViewModel>>
                (userService.FindAll(null, include));

            foreach(var user in users)
            {
                user.Password = "";
            }

            return View(users);
        }
    }
}