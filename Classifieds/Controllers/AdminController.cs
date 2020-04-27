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
using Classifieds.Domain.Data;
using Classifieds.Domain.Enumerated;
using System.Security.Claims;

namespace Classifieds.Web.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        private IMenuService menuService;
        private IUserService userService;
        private IAdvertService advertService;
        private ICategoryService categoryService;
        private ILikeService likeService;
        private IMapper mapper;

        public AdminController(IMenuService menuService, IUserService userService,
            IAdvertService advertService, ICategoryService categoryService,
            ILikeService likeService, IMapper mapper)
        {
            this.menuService = menuService;
            this.userService = userService;
            this.advertService = advertService;
            this.categoryService = categoryService;
            this.likeService = likeService;
            this.mapper = mapper;
        }
        /// <summary>
        /// Admin Dashboard Page
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            var userId = User.Claims.FirstOrDefault(x => x.Type == "UserId").Value;
            var roleClaims = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role).Value;
            List<RoleViewModel> roles = null;
            bool admin = false;

            if(roleClaims != null)
            {
                roles = JsonConvert.DeserializeObject<List<RoleViewModel>>(roleClaims);
            }
            else
            {
                roles = new List<RoleViewModel>
                {
                    new RoleViewModel
                    {
                        UserID = userId == null ? 0 : long.Parse(userId),
                        Name = "ROLE_USER"
                    }
                };
            }

            //get new adverts
            Expression<Func<Advert, bool>> adPredicate = a => a.Status == EnumTypes.AdvertStatus.SUBMITTED.ToString();
            Expression<Func<Advert, object>>[] adInclude = { a => a.Detail };
            var newAds = mapper.Map<IEnumerable<AdvertViewModel>>(advertService.FindAll(adPredicate, adInclude));

            List<CountPercentSummary> advertSummary = CountAdvertByStatus();

            //Admin Analytics
            if (roles.Any(r => r.Name == "ROLE_ADMIN"))
            {
                admin = true;

                //Total users and adverts
                ViewBag.CountUsers = userService.CountAllUsers();
                ViewBag.CountAdverts = advertService.CountAdverts(string.Empty);
                ViewBag.CountNewAdverts = advertService.CountAdverts("submitted");

                ViewBag.AdvertSummary = advertSummary;

                //Get New Users
                Expression<Func<User, bool>> userPredicate = a => a.IsVerified == 0;
                Expression<Func<User, object>>[] userInclude = { a => a.UserDetail };
                var newUsers = mapper.Map<IEnumerable<UserViewModel>>(userService.FindAll(userPredicate, userInclude));

                ViewBag.Adverts = newAds;
                ViewBag.Users = newUsers;

            }
            else if (roles.Any(r => r.UserID > 0))
            {
                //User Analytics
                ViewBag.CountAdverts = advertService.CountAdvertsByUser(long.Parse(userId));
                ViewBag.CountApprovedAds = advertService.CountAdvertsByUserByStatus(long.Parse(userId), "approved");
                ViewBag.CountFavourites = likeService.CountLikesByUser(long.Parse(userId));

                ViewBag.AdvertSummary = advertSummary;

                ViewBag.Adverts = newAds;

            }

            return admin ? View("Index") : View("Dashboard");
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
                m => m.UserDetail,
                m => m.Roles
            };

            var users= mapper.Map<IEnumerable<UserViewModel>>
                (userService.FindAll(null, include));

            foreach(var user in users)
            {
                user.Password = "";
            }

            return View(users);
        }
        /// <summary>
        /// /Admin/Categories
        /// </summary>
        /// <returns></returns>
        public IActionResult Categories()
        {
            Expression<Func<Category, bool>> where = c => c.ParentID == null;
            Expression<Func<Category, object>>[] include =
            {
                c => c.SubCategories
            };

            var categories = mapper.Map<IEnumerable<CategoryViewModel>>
                (categoryService.FindAll(where, include));


            return View(categories);
        }
        public IActionResult Preferences()
        {

            return View();
        }
        /// <summary>
        /// Favourites Page
        /// </summary>
        /// <returns>Model and View</returns>
        public IActionResult Favourites()
        {
            IEnumerable<LikeViewModel> favourites = null;
            try
            {
                var userId = long.Parse(User.Claims.FirstOrDefault(x => x.Type == "UserId").Value);
                favourites = mapper.Map<IEnumerable<LikeViewModel>>(likeService.FindByUser(userId));
            }
            catch(Exception ex)
            {
                //log exception
                Console.WriteLine("Exception in Favourites" + ex.StackTrace);
            }
            

            return View(favourites);
        }
        /// <summary>
        /// Calculate percentage and count of adverts grouped by AdvertStatus column
        /// </summary>
        /// <returns>Listcontaining summary results</returns>
        public List<CountPercentSummary> CountAdvertByStatus()
        {
            var userId = User.Claims.FirstOrDefault(x => x.Type == "UserId").Value;
            var roleClaims = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role).Value;

            if (roleClaims.Contains("ROLE_ADMIN"))
            {
                return advertService.AdvertCountByStatus();
            }
            else
            {
                long id = userId == null ? 0 : long.Parse(userId);

                return advertService.AdvertCountByStatusByUser(id);
            }

            
        }
        /// <summary>
        /// Calculate percentage and count of adverts grouped by AdvertStatus column
        /// </summary>
        /// <returns>Listcontaining summary results</returns>
        public List<CountPercentSummary> CountAdvertByLocation()
        {
            return advertService.AdvertCountByLocation();
        }
        /// <summary>
        /// Calculate percentage and count of adverts grouped by AdvertStatus column
        /// </summary>
        /// <returns>Listcontaining summary results</returns>
        public List<CountPercentSummary> CountAdvertsByStatusByUser()
        {
            var userId = User.Claims.FirstOrDefault(x => x.Type == "UserId").Value;

            long id = userId == null ? 0 : long.Parse(userId);

            return advertService.AdvertCountByStatusByUser(id);
        }
        /// <summary>
        /// Calculate percentage and count of adverts grouped by AdvertStatus column
        /// </summary>
        /// <returns>Listcontaining summary results</returns>
        public List<CountPercentSummary> CountAdvertByCategory()
        {
            return advertService.AdvertCountByCategory();
        }
    }
}