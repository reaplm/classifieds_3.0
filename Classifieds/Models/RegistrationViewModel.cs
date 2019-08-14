using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Classifieds.Web.Models
{
    public class RegistrationViewModel 
    {
        public UserViewModel User { set; get; }
        [Required(ErrorMessage = "Password is required")]
        public string Password { set; get; }
        [Compare(nameof(Password), ErrorMessage ="Passwords do not match")]
        public string ConfirmPassword { set; get; }
        [Range(typeof(bool),"true", "true",ErrorMessage ="Please accept terms and conditions")]
        public bool AcceptTerms { set; get; }
    }
}
