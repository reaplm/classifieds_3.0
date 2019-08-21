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
                User user = userService.Create(mapper.Map<User>(model.User));
                userService.Save();

                if (user != null)
                {
                    //confirm Registration
                    //Generate Token
                    string verificationToken = Guid.NewGuid().ToString();

                    if (userService.CreateVerificationToken(user.ID, verificationToken))
                    {
                        //send email
                        SendConfirmationEmail(user.Email, verificationToken);
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
        public IActionResult ConfirmRegistration()
        {
            return View();
        }
        /// <summary>
        /// After successful registration, send email for user activate account
        /// </summary>
        /// <param name="email">User's email</param>
        /// <param name="token">Verification token</param>
        /// <returns></returns>
        public Task SendConfirmationEmail(string email, string token)
        {
            string url = "localhost/Registration/ConfirmRegistration?token=" + token;
            string subject  = "Classifieds Registration";
            string message = "<p>Click the url below to activate your registration<p><p>" +
                "<a href='" + url + "'>" + url + "</a></p>";

            return userService.SendVerificationEmailAsync(email, subject, message);
        }
    }
}