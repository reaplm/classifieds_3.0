using Classifieds.Domain.Model;
using Classifieds.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace Classifieds.Service.Impl
{
    public class DeviceTypeService : GenericService<DeviceType>, IDeviceTypeService
    {
        IDeviceTypeRepo repo;

        public DeviceTypeService(IDeviceTypeRepo repo) : base(repo)
        {
            this.repo = repo;
        }
    }
}
