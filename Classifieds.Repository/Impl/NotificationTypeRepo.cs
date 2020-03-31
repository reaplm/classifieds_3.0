using Classifieds.Domain.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Classifieds.Repository.Impl
{
    public class NotificationTypeRepo : GenericRepo<NotificationType>, INotificationTypeRepo
    {
        private ApplicationContext context;

        public NotificationTypeRepo(ApplicationContext context) : base(context)
        {
            this.context = context;
        }
    }
}
