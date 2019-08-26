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
   
        public string FirstName { set; get; }

        [Column(name: "last_name" )]
        public string LastName { set; get; }

        [Column(name: "image_cdn")]
        public string ImageCdn { set; get; }

        [Column(name: "image_name")]
        public string ImageName { set; get; }

        [Column(name:"image_uuid")]
        public string ImageUuid { set; get; }

        [Column(name: "mobile_no")]
        public string MobileNo { set; get; }

        [Column(name: "fk_user_id")]
        public long UserID { set; get; }
        [ForeignKey("UserID")]
        public User User { set; get; }

        public virtual Address Address { set; get; }
    }
}
