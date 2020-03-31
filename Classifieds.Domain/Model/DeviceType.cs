using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Classifieds.Domain.Model
{
    [Table(name: "device_type")]
    public class DeviceType
    {
        [Key]
        [ReadOnly(true)]
        [Column(name: "pk_device_id")]
        public long ID { set; get; }
        [Column(name: "name")]
        public string Name { set; get; }
    }
}
