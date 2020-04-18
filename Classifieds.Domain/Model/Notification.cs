using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Classifieds.Domain.Model
{
    [Table(name: "notification")]
    public class Notification
    {
        [Key]
        [Column(name: "pk_not_id")]
        public long ID { set; get; }

        [ForeignKey("UserID")]
        public User User { set; get; }
        [Column(name: "fk_user_id")]
        public long UserID { set; get; }

        [ForeignKey("DeviceID")]
        public Device Device { set; get; }
        [Column(name: "fk_device_id")]
        public long DeviceID { set; get; }

        [ForeignKey("NotificationCatID")]
        public NotificationCategory NotificationCategory { set; get; }
        [Column(name: "fk_not_cat_id")]
        public long NotificationCatID { set; get; }

        [ForeignKey("NotificationTypeID")]
        public NotificationType NotificationType { set; get; }
        [Column(name: "fk_not_typ_id")]
        public long NotificationTypeID { set; get; }
    }
}
