using Classifieds.Domain.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Classifieds.Repository.Impl
{
    public class NotificationCategoryRepo : GenericRepo<NotificationCategory>, INotificationCategoryRepo
    {
        private ApplicationContext context;

        public NotificationCategoryRepo(ApplicationContext context) : base(context)
        {
            this.context = context;
        }
    }
}
