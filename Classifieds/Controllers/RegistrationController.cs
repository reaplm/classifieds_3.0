using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Classifieds.Domain.Enumerated;
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
            ViewBag.ReturnUrl = "/Registration/ConfirmRegistration/";
            return View();
        }
        /// <summary>
        /// Submit registration
        /// </summary>
        /// <param name="model">Form data</param>
        /// <param name="ReturnUrl">Url to redirect to</param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Index(RegistrationViewModel model, string ReturnUrl)
        {
            if (ModelState.IsValid)
            {
                //encrypt password
                model.User.Password = userService.GetEncryptedPassword(model.Password);
                model.User.RegDate = DateTime.Now;
                model.User.Roles = new List<RoleViewModel>
                {
                    new RoleViewModel{ Name=EnumTypes.Roles.ROLE_USER.ToString()}
                };
                var user = userService.Create(mapper.Map<User>(model.User));

                if (user.ID > 0)
                {
                    //confirm Registration
                    //Generate Token
                    string verificationToken = Guid.NewGuid().ToString();

                    if (userService.CreateVerificationToken(user.ID, verificationToken))
                    {
                        //send email
                        SendConfirmationEmail(model.User.Email, user.ID, verificationToken);
                    }

                    return Redirect(ReturnUrl);
                }
            }

            return View(model);
        }
        /// <summary>
        /// Registration success page
        /// </summary>
        /// <returns></returns>
        /// /Registration/ConfirmRegistration

        /// <summary>
        /// Registration success page
        /// </summary>
        /// <returns></returns>
        /// /Registration/ConfirmRegistration/{token}
        public IActionResult ConfirmRegistration(long id, string token)
        {
            if(token != null)
            {
                int changed = userService.ActivateAccount(id, token);
                ViewBag.IsActivated = true;

                if (changed > 0)
                    ViewBag.Error = false;
                else
                    //something went wrong
                    ViewBag.Error = true;
            }
            else
            {
                ViewBag.IsActivated = false;
            }
            return View();
        }
        /// <summary>
        /// After successful registration, send email for user activate account
        /// </summary>
        /// <param name="email">User's email</param>
        /// <param name="token">Verification token</param>
        /// <returns></returns>
        public Task SendConfirmationEmail(string email, long userId, string token)
        {
            var url = Url.Action("ConfirmRegistration", "Registration",
               new {id=userId, token = token },
               Request.Scheme);
            string subject  = "Classifieds Registration";
            string message = "<p>Click the url below to activate your registration<p><p>" +
                "<a href='" + url + "'>" + url + "</a></p>";

            return userService.SendVerificationEmailAsync(email, subject, message);
        }
    }
}