using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Classifieds.Web.Models
{
    public class UserViewModel
    {

        public long ID { set; get; }
        public int? Activated { set; get; }
        public String ActivationCode { set; get; }
        public int? Notified { set; get; }

        [Required(ErrorMessage = "Please enter email address")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public String Email { set; get; }

        public String Password { set; get; }

        public DateTime? LastLogin { set; get; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}",
               ApplyFormatInEditMode = true)]
        public DateTime RegDate { set; get; }
        public DateTime? PasswordExpiry { set; get; }

        public int IsVerified { set; get; }
        public string VerificationToken { set; get; }

        public virtual UserDetailViewModel UserDetail{set;get;}
        
    }
}
