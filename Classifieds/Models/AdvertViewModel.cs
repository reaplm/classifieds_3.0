﻿using Classifieds.Domain.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Classifieds.Web.Models
{
    public class AdvertViewModel
    {

        public long ID { set; get; }
        public String Status { set; get; }

        public DateTime ApprovedDate { set; get; }
        public DateTime PublishedDate { set; get; }
        public DateTime RejectedDate { set; get; }
        public DateTime SubmittedDate { set; get; }

        [Required(ErrorMessage = "Please select sub-menu")]
        [Range(1,int.MaxValue,ErrorMessage = "Please select sub-menu")]
        public long MenuID { set; get; }
        [Display(Name = "Menu")]
        public MenuViewModel Menu { set; get; }

        [Required(ErrorMessage = "Please select menu")]
        [Range(1, int.MaxValue, ErrorMessage = "Please select menu")]
        public int ParentID { set; get; }

        [Required(ErrorMessage ="Please login to post an ad")]
        public long UserID { set; get; }
        public virtual UserViewModel User { set; get; }

        public AdvertDetailViewModel Detail { set; get; }
    }
}
