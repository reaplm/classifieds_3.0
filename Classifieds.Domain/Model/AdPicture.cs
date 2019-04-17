using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Classifieds.Domain.Model
{
    [Table(name: "ad_picture")]
    public class AdPicture { 

        [Column(name:"ad_pic_id")]
        public long ID { set; get; }

        [Column(name: "cdn_url")]
        public String CdnUrl { set; get; }

        [Column(name: "uuid")]
        public String Uuid { set; get; }

        [Column(name: "name")]
        public String Name { set; get; }

        [Column(name: "size")]
        public long Size { set; get; }

        [Column(name: "fk_ad_detail")]
        public long AdvertDetailID { set; get; }
        [ForeignKey("AdvertDetailID")]
        public virtual AdvertDetail AdvertDetail { set; get; }
    }
}
