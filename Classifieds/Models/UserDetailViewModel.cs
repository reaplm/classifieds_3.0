using Classifieds.Domain.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Classifieds.Web.Models
{
    public class UserDetailViewModel
    {
        public long ID { set; get; }

        [Display(Name = "First Name")]
        [Required(ErrorMessage = "Please enter first name")]
        public String FirstName { set; get; }

        [Display(Name ="Last Name")]
        [Required(ErrorMessage = "Please enter last name")]
        public String LastName { set; get; }

        public String ImageCdn { set; get; }
        public String ImageName { set; get; }
        public String ImageUuid { set; get; }

        [Display(Name = "Mobile No")]
        [Required(ErrorMessage = "Mobile Number is required")]
        public String MobileNo { set; get; }

        public long UserID { set; get; }
        public UserViewModel User { set; get; }

        public virtual AddressViewModel Address { set; get; }
    }
}
