using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Classifieds.Domain.Model
{
    [Table(name: "like")]
    public class Like
    {
        [Key]
        [Column(name: "pk_like_id")]
        public long ID { set; get; }

        [Column(name: "like_date")]
        public DateTime LikeDate { set; get; }

        [Column(name: "fk_user_id")]
        public long UserID { set; get; }
        [ForeignKey("UserID")]
        public User User { set; get; }

        [Column(name: "fk_advert_id")]
        public long AdvertID { set; get; }
        [ForeignKey("AdvertID")]
        public Advert Advert { set; get; }
    }
}
