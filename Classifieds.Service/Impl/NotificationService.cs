using Classifieds.Domain.Model;
using Classifieds.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace Classifieds.Service.Impl
{
    public class NotificationService : GenericService<Notification>, INotificationService
    {
        INotificationRepo repo;

        public NotificationService(INotificationRepo repo) : base(repo)
        {
            this.repo = repo;
        }
    }
}
