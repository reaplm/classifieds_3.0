using Classifieds.Service;
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
        public IActionResult Index()
        {
            return View();
        }
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

                //save confirmation code

                string message = "<p>Dear User, " + 
                    "<p>A password reset request has been submitted for this E - Mail Address " +
                    "and its corresponding account. By using the confirmation-code " +
                    "you will now be able to reset the password for your " +
                    "account. </p>" +
                    "<p>Confirmation Code: " + resetCode + "</p>" +
                    "<p>The code expires in one hour.<p>" +
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
                return RedirectToAction("ResetSuccess");
            }
        }
        public IActionResult ResetSuccess()
        {
            return View();
        }
    }
}
