using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Classifieds.Domain.Model
{
    [Table(name:"address")]
    public class Address
    {
        [Key]
        [Column(name: "pk_address_id")]
        public long ID { set; get; }

        [Column(name: "post_addr1")]
        public string PostAddress1 { set; get; }

        [Column(name: "post_addr2")]
        public string PostAddress2 { set; get; }

        [Column(name: "post_code")]
        public string PostCode { set; get; }

        [Column(name: "state")]
        public string State { set; get; }

        [Column(name: "street")]
        public string Street { set; get; }

        [Column(name: "surbub")]
        public string Surbub { set; get; }

        [Column(name: "country")]
        public string Country { set; get; }

        [Column(name: "fk_detail_id")]
        public long DetailID { set; get; }
        [ForeignKey("DetailID")]
        public virtual UserDetail UserDetail { set; get; }

    }
}
