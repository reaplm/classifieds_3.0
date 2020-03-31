using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Classifieds.Domain.Model;
using Classifieds.Service;
using Classifieds.Web.Model;
using Classifieds.Web.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Classifieds.Web.Controllers
{
    public class LoginController : Controller
    {
        private IUserService userService;
        private IMenuService menuService;
        private INotificationCategoryService ncService;
        private INotificationTypeService ntService;
        private IMapper mapper;

        public LoginController(IUserService userService, IMenuService menuService, 
            INotificationCategoryService ncService, INotificationTypeService ntService,
            IMapper mapper)
        {
            this.userService = userService;
            this.menuService = menuService;
            this.ncService = ncService;
            this.ntService = ntService;
            this.mapper = mapper;
        }
        /// <summary>
        /// /Login
        /// </summary>
        /// <returns></returns>
        public IActionResult Index(String ReturnUrl)
        {
            ViewBag.ReturnUrl = ReturnUrl;

            return View();
        }
        /// <summary>
        /// Login Post
        /// </summary>
        /// <param name="user">ViewModel</param>
        /// <param name="ReturnUrl">Redirect to url after login</param>
        /// <returns></returns>
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Login(UserViewModel user, String ReturnUrl)
        {
            UserViewModel authenticatedUser = mapper.Map<UserViewModel>
                (userService.AuthenticateUser(user.Email, user.Password));

            if (authenticatedUser == null){
                ModelState.AddModelError("Password", "Wrong email or password");
            }
            else if (authenticatedUser.IsVerified == 0)
            {
                ModelState.AddModelError("Activated", "This account is not activated");
            }
            else if (ModelState.IsValid)
            {
                var claims = new List<Claim>
                {
                    new Claim("UserId", authenticatedUser.ID.ToString()),
                    new Claim(ClaimTypes.Name, authenticatedUser.UserDetail.FirstName),
                    new Claim(ClaimTypes.Email, authenticatedUser.Email),
                    new Claim(ClaimTypes.Role, "Administrator"),
                    new Claim("LastLoginDate", authenticatedUser.LastLogin.ToString()),
                    new Claim("ImageCdn", authenticatedUser.UserDetail.ImageCdn),
                    new Claim("Likes", JsonConvert.SerializeObject(authenticatedUser.Likes,new JsonSerializerSettings
                        {
                            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                        }))
                };
                var claimsIdentity = new ClaimsIdentity(claims, 
                    CookieAuthenticationDefaults.AuthenticationScheme);
                var authProperties = new AuthenticationProperties
                {
                    AllowRefresh= true
                };

                await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(claimsIdentity), authProperties);

                SetSessionVariables();

                return RedirectToLocal(ReturnUrl);
            }


            ViewBag.ReturnUrl = ReturnUrl;
            return View("Index", user);
        }
        /// <summary>
        /// Signout
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> SignOut()
        {
            await HttpContext.SignOutAsync(
                CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Index", "Home");
        }
        /// <summary>
        /// Redirect to url
        /// </summary>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else { return RedirectToAction("Index", "Home"); }
        }
        /// <summary>
        /// Set session variables
        /// </summary>
        private void SetSessionVariables()
        {

            Expression<Func<Menu, bool>> where = m => m.ParentID == null;
            Expression<Func<Menu, object>>[] include =
            {
                m => m.SubMenus
            };

            IEnumerable<MenuViewModel> menus = mapper.Map<IEnumerable<MenuViewModel>>
                    (menuService.FindAll(where, include));

            IEnumerable<NotificationTypeViewModel> notificationTypes = mapper.Map<IEnumerable<NotificationTypeViewModel>>
                (ntService.FindAll());

            IEnumerable<NotificationCategoryViewModel> notificationCategories =
                mapper.Map<IEnumerable<NotificationCategoryViewModel>>(ncService.FindAll());

            HttpContext.Session.SetString("SideMenus", JsonConvert.SerializeObject(menus,
                Formatting.Indented, new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                }));

            HttpContext.Session.SetString("NotificationTypes", JsonConvert.SerializeObject(notificationTypes,
                Formatting.Indented, new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                }));

            HttpContext.Session.SetString("NotificationCategories", JsonConvert.SerializeObject(notificationCategories,
                Formatting.Indented, new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                }));


        }
    }
}