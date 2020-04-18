using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Classifieds.Web.Models
{
    public class NotificationViewModel
    {
        public long ID { set; get; }

        public DeviceViewModel Device { set; get; }
        public long DeviceID { set; get; }

        public NotificationCategoryViewModel NotificationCategory { set; get; }
        public long NotificationCatID { set; get; }

        public NotificationTypeViewModel NotificationType { set; get; }
        public long NotificationTypeID { set; get; }
    }
}
