using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using Classifieds.Domain.Model;
using Classifieds.Service;
using Classifieds.Service.Impl;
using Classifieds.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace Classifieds.Web.Controllers
{
    public class ProfileController : Controller
    {
        private IUserService userService;
        private IMapper mapper;

        public ProfileController(IUserService userService, IMapper mapper)
        {
            this.userService = userService;
            this.mapper = mapper;
        }
        /// <summary>
        /// Profile page
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            //fetch User
            var userId = HttpContext.User.Claims.FirstOrDefault(u => u.Type == "UserId").Value;

            Expression<Func<User, Object>>[] include =
            {
                u => u.UserDetail
            };

            UserViewModel user = mapper.Map<UserViewModel>(userService.Find(long.Parse(userId), include));

            return View(user);
        }
    }
}