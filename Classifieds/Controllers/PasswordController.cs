using Classifieds.Service;
using Classifieds.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Classifieds.Web.Controllers
{
    public class PasswordController : Controller
    {
        private IUserService userService;

        public PasswordController(IUserService userService)
        {
            this.userService = userService;
        }
        /// <summary>
        /// Index page
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// The user enters and submits the email address they used to register an account.
        /// If the email address does not exist in the system an error message is displayed
        /// in the page.
        /// If the email address is found and email is sent to the user with instructions
        /// on how to reset their password. A random code is also generated and will be used
        /// to verify the user during reset.
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(string email)
        {
            var user = userService.ValidateEmailAddress(email);

            if(user == null)
            {
                ViewBag.Error = true;
                ViewBag.Message = "The email address you entered does not exist. " +
                    "You can either try another email address, or log in through " +
                    "<a>Facebook or Google</a> - if your account is connected.";
                return View();
            }
            else
            {
                string resetCode = userService.RandomCodeGenerator();

                //save reset code
                userService.UpdateResetCode(user.ID, resetCode);   

                var url = Url.Action("Reset", "Password",
                    new { id = user.ID}, Request.Scheme);

                string message = "<p>Dear User, " + 
                    "<p>A password reset request has been submitted for this E - Mail Address " +
                    "and its corresponding account. By using the confirmation-code " +
                    "you will now be able to reset the password for your account. </p> " +
                    "<p>Reset Code: " + resetCode + "</p>" +
                    "<p>The code expires in one hour.<p>" +
                    "Click or paste the link below. Enter the reset code above to reset your password</p>" +
                    "<p></p><p><a href='" + url +"'>" + url + "</a></p>" +
                    "<p></p><p>This confirmation code has been sent to the following E - Mail addresses: " +
                    "pdm.molefe @gmail.com </p>" +
                    "<p>If you did not submit this request and unauthorized persons have access to one " +
                    "of the email addresses above, please contact IT - Support(by responding to this " +
                    "E - Mail) immediatly. In other cases, please ignore this E - Mail.</p><p></p>" +
                    "<p>---------------------------------------------------------------------------------</p>" +
                    "<p>Regards,</p>" + 
                    "<p>Classifieds Support</p>";

                //send email
                Task task = userService.SendEmailAsync(user.Email, "Request for password reset", message);
                if(!task.IsCompleted || !task.IsCompletedSuccessfully)
                {
                    //log this error
                }
                return RedirectToAction("EmailSuccess", new {id=user.ID});
            }
        }

        /// <summary>
        /// success page after sending email to the user for resetting their password
        /// </summary>
        /// <returns></returns>
        public IActionResult EmailSuccess()
        {
            return View();
        }
        /// <summary>
        /// Form for resetting password
        /// </summary>
        /// <param name="id">User's id</param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Reset(long id)
        {
            var user = userService.Find(id);
            var model = new PasswordResetViewModel { Email=user.Email,ID=user.ID};

            return View(model);
        }
        /// <summary>
        /// Submit method containing new password
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Reset(PasswordResetViewModel model)
        {
            //compare code with the one in the database
            var user= userService.Find(model.ID);
            if (!user.ResetCode.Equals(model.ResetCode))
            {
                ModelState.AddModelError("ResetCode", "The code you entered is incorrect. The code is case sensitive");
            }

            if (ModelState.IsValid)
            {
                //update password
                user.Password = userService.GetEncryptedPassword(model.Password);
                userService.Update(user);

                //redirect to success page
                return RedirectToAction("ResetSuccess");
            }

            return View(model);
        }
        /// <summary>
        /// After the user has successfully reset their password
        /// </summary>
        /// <returns></returns>
        public IActionResult ResetSuccess()
        {
            return View();
        }
    }
}
