using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Classifieds.Web.Models
{
    public class AdPictureViewModel
    {
        public long ID { set; get; }
        public String CdnUrl { set; get; }
        public String Uuid { set; get; }
        public String Name { set; get; }
        public long Size { set; get; }

        public long AdvertID { set; get; }
        public virtual AdvertViewModel Advert { set; get; }
    }
}
