using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Classifieds.Web.Models
{
    public class CategoryViewModel
    {

        public long ID { set; get; }

        [Required(ErrorMessage ="Category name is required")]
        public String Name { set; get; }
        public String Desc;

        public String Label { set; get; }

        [Required(ErrorMessage = "Url is required")]
        public String Url { set; get; }
        public String Icon { set; get; }

        public bool Status { set; get; }
 
        public long ParentID { set; get; }
        public virtual CategoryViewModel Parent { set; get; }

        public virtual List<CategoryViewModel> SubCategories { set; get; }
        public virtual List<AdvertViewModel> Adverts { set; get; }
    }
}
