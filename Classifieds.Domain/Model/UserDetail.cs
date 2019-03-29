using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Classifieds.Domain.Model
{
    [Table(name: "user_detail")]
    public class UserDetail
    {
        [Key]
        [Column(name: "pk_user_detail_id")]
        public long ID { set; get; }

        [Column(name: "first_name")]
        public String FirstName { set; get; }

        [Column(name: "last_name" )]
        public String LastName { set; get; }

        [Column(name: "image_cdn")]
        public String ImageCdn { set; get; }

        [Column(name: "image_name")]
        public String ImageName { set; get; }

        [Column(name:"image_uuid")]
        public String ImageUuid { set; get; }

        [Column(name: "mobile_no")]
        public String MobileNo { set; get; }

        [Column(name: "fk_user_id")]
        public long UserID { set; get; }
        [ForeignKey("UserID")]
        public User User { set; get; }
    }
}
