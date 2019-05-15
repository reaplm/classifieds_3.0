using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Classifieds.Domain.Model;
using Classifieds.Service;
using Classifieds.Service.Impl;
using Classifieds.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Classifieds.Web.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private IUserService userService;
        private IAddressService addressService;
        private IUserDetailService userDetailService;
        private IMapper mapper;

        public ProfileController(IUserService userService, 
            IAddressService addressService,
            IUserDetailService userDetailService,
            IMapper mapper)
        {
            this.userService = userService;
            this.addressService = addressService;
            this.userDetailService = userDetailService;
            this.mapper = mapper;
        }
        /// <summary>
        /// /Profile/EditAddress
        /// </summary>
        /// <returns></returns>
        public IActionResult EditAddress()
        {
            var userId = HttpContext.User.Claims.FirstOrDefault(u => u.Type == "UserId").Value;

            Expression<Func<UserDetail, bool>> where =
                u => u.ID == long.Parse(userId);
            Expression<Func<UserDetail, object>>[] include =
            {
                u => u.Address
            };

            UserDetailViewModel model = mapper.Map< UserDetailViewModel>
                (userDetailService.Find(where, include));
            

            return PartialView(model);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditAddress(UserDetailViewModel model)
        {
            if (ModelState.IsValid)
            {
                //remove all addresses
                Expression<Func<UserDetail, object>>[] include =
                {
                    u => u.Address
                };

                UserDetail userDetail = userDetailService.Find(model.ID,include);
                addressService.Delete(userDetail.Address.ID);
                addressService.Save();

                userDetail.Address = mapper.Map<Address>(model.Address);
                userDetailService.Update(userDetail);
                userDetailService.Save();

                HttpContext.Response.StatusCode = (int)HttpStatusCode.Created;
                return new JsonResult("Address Saved!");
            }

            HttpContext.Response.StatusCode = (int)HttpStatusCode.OK;
            return PartialView(model);
        }
    }
}