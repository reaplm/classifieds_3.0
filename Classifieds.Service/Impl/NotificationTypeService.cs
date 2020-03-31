using Classifieds.Domain.Model;
using Classifieds.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace Classifieds.Service.Impl
{
    public class NotificationTypeService : GenericService<NotificationType>, INotificationTypeService
    {
        INotificationTypeRepo repo;

        public NotificationTypeService(INotificationTypeRepo repo) : base(repo)
        {
            this.repo = repo;
        }
    }
}
