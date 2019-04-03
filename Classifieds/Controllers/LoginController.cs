using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Classifieds.Domain.Model;
using Classifieds.Service;
using Classifieds.Web.Models;
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
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Login(UserViewModel user)
        {
            UserViewModel autenticatedUser = mapper.Map<UserViewModel>
                (userService.authenticateUser(user.Email, user.Password));

            if (autenticatedUser  == null){
                ModelState.AddModelError("Password", "Wrong email or password");
            }
            if (ModelState.IsValid)
            {
                return RedirectToAction("Index", "Home", "");
            }

            return View("Index", user);
        }
        

        
    }
}