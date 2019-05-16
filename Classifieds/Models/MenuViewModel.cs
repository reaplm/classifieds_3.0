using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Classifieds.Web.Models
{
    public class MenuViewModel
    {

        public int ID { set; get; }

        [Required(ErrorMessage = "Name is required")]
        [Display(Name ="Name")]
        public String Name { set; get; }

        [Required(ErrorMessage = "Required")]
        [Display(Name = "Admin")]
        public bool Admin { set; get; }

        [Display(Name = "Icon")]
        public String Icon { set; get; }

        [Display(Name = "Label")]
        public String Label { set; get; }

        [Display(Name = "Desc")]
        public String Desc { set; get; }

        [Display(Name = "Status")]
        public bool Active { set; get; }

        [Display(Name = "Menu Type")]
        public String Type { set; get; }

        [Required(ErrorMessage = "Url is required")]
        [Display(Name = "Url")]
        public String Url { set; get; }

        public int? ParentID { set; get; }
        public virtual MenuViewModel Parent { set; get; }

        public virtual IEnumerable<MenuViewModel> SubMenus { set; get; }
    }
}
