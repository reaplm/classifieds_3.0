using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Classifieds.Web.Models
{
    [Table(name: "like")]
    public class LikeViewModel
    {
        public long ID { set; get; }

        public long UserID { set; get; }
        public UserViewModel User { set; get; }

        public long AdvertID { set; get; }
        public AdvertViewModel Advert { set; get; }
    }
}
