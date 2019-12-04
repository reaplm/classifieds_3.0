using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Classifieds.Web.Models
{
    public class DashboardViewModel
    {
        public int CountUsers { set; get; }
        public int CountAdverts { set; get; }
        public List<UserViewModel> Users{set;get;}
        public List<AdvertViewModel> Adverts { set; get; }

    }
}
