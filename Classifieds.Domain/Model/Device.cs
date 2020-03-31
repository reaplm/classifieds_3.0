using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Classifieds.Domain.Model
{
    public class Device
    {
        [Key]
        public long ID { set; get; }
        public bool IsEnabled { set; get; }

        [ForeignKey("UserID")]
        public User User { set; get; }
        [Column(name: "fk_user_id")]
        public long UserID { set; get; }

        [ForeignKey("DeviceTypeID")]
        public DeviceType DeviceType { set; get; }
        [Column(name: "fk_dev_type_id")]
        public long DeviceTypeID { set; get; }
    }
}
