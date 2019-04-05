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
       // private readonly UserManager<User> userManager;
        //private readonly SignInManager<User> signInManager;

        public LoginController(IUserService userService, IMapper mapper)
        {
            this.userService = userService;
            this.mapper = mapper;
            //this.userManager = userManager;
            //this.signInManager = signInManager;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Login(UserViewModel user)
        {
            UserViewModel authenticatedUser = mapper.Map<UserViewModel>
                (userService.authenticateUser(user.Email, user.Password));

            //User authUser = userService.authenticateUser(user.Email, user.Password);

            if (authenticatedUser == null){
                ModelState.AddModelError("Password", "Wrong email or password");
            }
            if (ModelState.IsValid)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.Email),
                    new Claim(ClaimTypes.Email, user.Email),
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

                return RedirectToAction("Index", "Home", "");
            }

            return View("Index", user);
        }
        

        
    }
}