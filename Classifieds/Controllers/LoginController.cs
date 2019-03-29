using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Classifieds.Service;
using Classifieds.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace Classifieds.Web.Controllers
{
    public class LoginController : Controller
    {
        private IUserService userService;

        public LoginController(IUserService userService)
        {
            this.userService = userService;
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}