using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Classifieds.Web.Models
{
    public class DeviceViewModel
    {
        public long ID { set; get; }
        public bool IsEnabled { set; get; }

        public UserViewModel User { set; get; }
        public long UserID { set; get; }

        public DeviceTypeViewModel DeviceType { set; get; }
        public long DeviceTypeID { set; get; }
    }
}
