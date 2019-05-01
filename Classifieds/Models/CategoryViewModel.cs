using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Classifieds.Web.Models
{
    public class CategoryViewModel
    {

        public long ID { set; get; }
        public String Name { set; get; }
        public String Desc;

        public String Label { set; get; }
        public String Url { set; get; }
        public String Icon { set; get; }
 
        public long ParentID { set; get; }
        public virtual CategoryViewModel Parent { set; get; }

        public virtual List<CategoryViewModel> SubCategories { set; get; }
        public virtual List<AdvertViewModel> Adverts { set; get; }
    }
}
