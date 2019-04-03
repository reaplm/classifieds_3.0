using Classifieds.Domain.Model;
using System;


namespace Classifieds.Web.Models
{
    public class AdvertDetailViewModel
    {

        public long ID { set; get; }
        public String Title { set; get; }
        public String Body { set; get; }
        public String Email { set; get; }
        public String Phone { set; get; }
        public String GroupCdn { set; get; }
        public int GroupCount { set; get; }
        public long GroupSize { set; get; }
       public String GroupUuid { set; get; }
        public String Location { set; get; }
        public long AdvertID { set; get; }
        public Advert Advert { set; get; }
    }
}
