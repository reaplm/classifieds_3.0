using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Classifieds.Web.Models
{
    public class Menu
    {

        public int ID { set; get; }

        [Required(ErrorMessage = "Menu name is required")]
        public String Name { set; get; }

        [Required(ErrorMessage = "Required")]
        public bool Admin { set; get; }

        public String Icon { set; get; }

        [Required]
        public String Label { set; get; }

        public String Desc { set; get; }

        public int Status { set; get; }

        public String Type { set; get; }

        public String Url { set; get; }

        public int? ParentID { set; get; }
        public virtual Menu Parent { set; get; }

        public virtual List<Menu> SubMenus { set; get; }
    }
}
