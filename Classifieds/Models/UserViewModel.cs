﻿using System;
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

        [Required(ErrorMessage ="Please enter email address")]
        [EmailAddress(ErrorMessage ="Invalid email address")]
        public String Email { set; get; }

        [Required(ErrorMessage = "Password is required")]
        public String Password { set; get; }

        public DateTime? LastLogin { set; get; }
        public DateTime RegDate { set; get; }
        public DateTime? PasswordExpiry { set; get; }

        
    }
}