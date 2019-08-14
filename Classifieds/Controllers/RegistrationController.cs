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
    public class RegistrationController : Controller
    {
        private IMapper mapper;
        private IUserService userService;

        public RegistrationController(IMapper mapper, IUserService userService)
        {
            this.mapper = mapper;
            this.userService = userService;
        }
        /// <summary>
        /// Create a new account
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            ViewBag.ReturnUrl = "/Login/";
            return View();
        }
        [HttpPost]
        public IActionResult Index(RegistrationViewModel model, string ReturnUrl)
        {
            if (ModelState.IsValid)
            {
                //encrypt password
                model.User.Password = userService.GetEncryptedPassword(model.Password);
                model.User.RegDate = DateTime.Now;
                userService.Create(mapper.Map<User>(model.User));
                userService.Save();

                return Redirect(ReturnUrl);
            }

            return View(model);
        }
    }
}