using Classifieds.Domain.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Classifieds.Repository.Impl
{
    public class DeviceTypeRepo : GenericRepo<DeviceType>, IDeviceTypeRepo
    {
        private ApplicationContext context;

        public DeviceTypeRepo(ApplicationContext context) : base(context)
        {
            this.context = context;
        }
    }
}
