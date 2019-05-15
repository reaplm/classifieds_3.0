using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Classifieds.Web.Models
{
    public class AddressViewModel
    {
        public long ID { set; get; }

        [Required(ErrorMessage ="Post Address1 is required")]
        [Display(Name ="Post Address 1")]
        public string PostAddress1 { set; get; }

        [Display(Name = "Post Address 2")]
        public string PostAddress2 { set; get; }

        [Display(Name = "Post Code")]
        public string PostCode { set; get; }

        [Required(ErrorMessage = "State is required")]
        [Display(Name = "State")]
        public string State { set; get; }

        [Required(ErrorMessage = "Please provide physical address")]
        [Display(Name="Street Address")]
        public string Street { set; get; }

        [Required(ErrorMessage = "Surbub is required")]
        [Display(Name = "Surbub")]
        public string Surbub { set; get; }

        [Display(Name = "Country")]
        public string Country { set; get; }

        public long DetailID { set; get; }
        public virtual UserDetailViewModel UserDetail { set; get; }

    }
}
