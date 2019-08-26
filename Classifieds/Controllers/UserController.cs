using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Classifieds.Service;
using Microsoft.AspNetCore.Mvc;

namespace Classifieds.Web.Controllers
{
    public class UserController : Controller
    { 

        private IUserService userService;

        public UserController(IUserService userService)
        {
            this.userService = userService;
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
    }
}