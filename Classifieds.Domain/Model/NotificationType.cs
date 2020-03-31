using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Classifieds.Domain.Model
{
    [Table(name: "notification_type")]
    public class NotificationType
    {
        [Key]
        [Column(name: "pk_not_typ_id")]
        public long ID { set; get; }
        [Column(name: "name")]
        public string Name { set; get; }
    }
}
