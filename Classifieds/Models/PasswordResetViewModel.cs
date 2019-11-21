using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Classifieds.Web.Models
{
    public class PasswordResetViewModel
    { 
        public long ID { set; get; }
        [Display(Name = "Email")]
        [Required(ErrorMessage = "Email is required", AllowEmptyStrings = false)]
        public string Email { set; get; }
        [Display(Name = "Reset Code")]
        [Required(ErrorMessage ="Please enter reset code",AllowEmptyStrings =false)]
        public string ResetCode { set; get; }
        [Display(Name = "Password")]
        [Required(ErrorMessage = "Please enter new password", AllowEmptyStrings = false)]
        public string Password { set; get; }
        [Display(Name = "Confirm Password")]
        [Compare("Password",ErrorMessage ="Passwords must match")]
        public string ConfirmPassword { set; get; }
    }
}
