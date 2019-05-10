using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Classifieds.Web.Models
{
    public class AdminViewModel
    {
        public virtual UserViewModel User { set; get; }
        public virtual IEnumerable<MenuViewModel> Menus { set; get; }
    }
}
