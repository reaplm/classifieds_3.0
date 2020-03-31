using Classifieds.Domain.Model;
using Classifieds.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace Classifieds.Service.Impl
{
    public class NotificationCategoryService : GenericService<NotificationCategory>, INotificationCategoryService
    {
        INotificationCategoryRepo repo;

        public NotificationCategoryService(INotificationCategoryRepo repo) : base(repo)
        {
            this.repo = repo;
        }
    }
}
