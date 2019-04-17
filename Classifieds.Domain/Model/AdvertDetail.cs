﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Classifieds.Domain.Model
{
    [Table(name:"advert_detail")]
    public class AdvertDetail
    {
        [Key]
        [Column(name: "pk_detail_id")]
        public long ID { set; get; }

        [Column(name: "title")]
        public String Title { set; get; }

        [Column(name: "body")]
        public String Body { set; get; }

        [Column(name: "contact_email")]
        public String Email { set; get; }

        [Column(name: "contact_phone")]
        public String Phone { set; get; }

        [Column(name: "group_cdn")]
        public String GroupCdn { set; get; }

        [Column(name: "group_count")]
        public int GroupCount { set; get; }

        [Column(name: "group_size")]
        public long? GroupSize { set; get; }

        [Column(name: "group_uuid")]
        public String GroupUuid { set; get; }

        [Column(name: "location")]
        public String Location { set; get; }

        [Column(name: "fk_advert_id")]
        public long AdvertID { set; get; }
        [ForeignKey("AdvertID")]
        public Advert Advert { set; get; }

        public virtual IEnumerable<AdPicture> AdPictures { set; get; }
    }
}
