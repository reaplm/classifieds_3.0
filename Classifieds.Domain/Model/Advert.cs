using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Classifieds.Domain.Model
{
    [Table(name: "advert")]
    public class Advert
    {
        [Key]
        [Column(name: "pk_advert_id")]
        [ReadOnly(true)]
        public long ID { set; get; }

        [Column(name: "advert_status")]
        public String Status { set; get; }

        [Column(name: "approved_date")]
        public DateTime? ApprovedDate {set;get;}

        [Column(name: "published_date")]
        public DateTime? PublishedDate { set; get; }

        [Column(name: "rejected_date")]
        public DateTime? RejectedDate { set; get; }

        [Column(name: "submitted_date")]
        public DateTime SubmittedDate { set; get; }
        
        [Column(name:"fk_cat_id")]
        public long CategoryID { set; get; }

        [ForeignKey("CategoryID")]
        public Category Category { set; get; }

        [Column(name: "fk_appuser_id")]
        [ReadOnly(true)]
        public long UserID { set; get; }

        public virtual User User { set; get; }

        public virtual AdvertDetail Detail { set; get; }
    }
}
