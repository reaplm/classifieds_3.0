using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using Classifieds.Domain.Model;
using Classifieds.Service;
using Classifieds.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Classifieds.Web.Controllers
{
    public class UserController : Controller
    { 

        private IUserService userService;
        private IMapper mapper;

        public UserController(IUserService userService, IMapper mapper)
        {
            this.userService = userService;
            this.mapper = mapper;
        }
        public IActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// Delete a user
        /// /User/Delete/id
        /// </summary>
        /// <param name="id">id of the user</param>
        /// <returns></returns>
        public IActionResult Delete(long id)
        {
            int changed = userService.Delete(id);

            if (changed > 0)
                return new JsonResult("Delete Successful!");
            else return new JsonResult("Delete Failed. Sorry.");
        }
        /// <summary>
        /// Return user details
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize]
        public IActionResult Detail(long id)
        {
            Expression<Func<User, object>>[] include =
            {
                a => a.UserDetail
            };
            UserViewModel model = mapper.Map<UserViewModel>(userService.Find(id, include));

            return PartialView(model);
        }
       
    }
}