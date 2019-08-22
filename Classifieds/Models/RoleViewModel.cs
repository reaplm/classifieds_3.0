using System;


namespace Classifieds.Web.Models
{
    public class RoleViewModel
    {
        public UserViewModel User { set; get; }
        public long ID { set; get; }
        public long UserID { set; get; }
        public string Name { set; get; }
    }
}
