using Classifieds.Domain.Model;
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

        [Display(Name = "Approved")]
        public DateTime? ApprovedDate { set; get; }

        [Display(Name = "Published")]
        public DateTime? PublishedDate { set; get; }

        [Display(Name = "Rejected")]
        public DateTime? RejectedDate { set; get; }

        [Display(Name = "Submitted")]
        public DateTime SubmittedDate { set; get; }

        [Required(ErrorMessage = "Please select sub-category")]
        [Range(1,int.MaxValue,ErrorMessage = "Please select sub-category")]
        public long CategoryID { set; get; }

        [Display(Name = "Category")]
        public CategoryViewModel Category { set; get; }

        [Required(ErrorMessage = "Please select category")]
        [Range(1, int.MaxValue, ErrorMessage = "Please select category")]
        public int ParentID { set; get; }

        [Required(ErrorMessage ="Please login to post an ad")]
        public long UserID { set; get; }
        public virtual UserViewModel User { set; get; }

        public AdvertDetailViewModel Detail { set; get; }

        public int Days
        {
            get
            {
               DateTime now = DateTime.Now;
                TimeSpan days = now - SubmittedDate;
                return days.Days;
            }
        }

    }
}
