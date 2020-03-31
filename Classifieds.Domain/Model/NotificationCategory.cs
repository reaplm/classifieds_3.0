using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Classifieds.Domain.Model
{
    [Table(name: "notification_category")]
    public class NotificationCategory
    {
        [Key]
        [Column(name: "pk_not_cat_id")]
        [ReadOnly(true)]
        public long ID { set; get; }
        [Column(name: "name")]
        public string Name { set; get; }
        [Column(name: "description")]
        public string Description { set; get; }
    }
}
