using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Classifieds.Domain.Model;
using Classifieds.Service;
using Classifieds.Web.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Classifieds.Web.Controllers
{
    public class LoginController : Controller
    {
        private IUserService userService;
        private IMapper mapper;

        public LoginController(IUserService userService, IMapper mapper)
        {
            this.userService = userService;
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
                (userService.authenticateUser(user.Email, user.Password));

            if (authenticatedUser == null){
                ModelState.AddModelError("Password", "Wrong email or password");
            }
            if (ModelState.IsValid)
            {
                var claims = new List<Claim>
                {
                    new Claim("UserId", authenticatedUser.ID.ToString()),
                    new Claim(ClaimTypes.Name, authenticatedUser.Email),
                    new Claim(ClaimTypes.Email, authenticatedUser.Email),
                    new Claim(ClaimTypes.Role, "Administrator")
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
        
    }
}