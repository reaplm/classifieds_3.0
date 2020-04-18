using Classifieds.Domain.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Classifieds.Repository.Impl
{
    public class NotificationRepo : GenericRepo<Notification>, INotificationRepo
    {
        private ApplicationContext context;

        public NotificationRepo(ApplicationContext context) : base(context)
        {
            this.context = context;
        }
    }
}
