using System;
using System.Collections.Generic;
using System.Text;

namespace Classifieds.Domain.Enumerated
{
    public class EnumTypes
    {
        public enum MenuType
        {
            HOME,
            SIDEBAR,
            SUBMENU
        }

        public enum Status
        {
            ACTIVE,
            INACTIVE
        }

        public enum AdvertStatus
        {
            SUBMITTED,
            REJECTED,
            APPROVED
        }

        public enum AddressType
        {
            MAILING,
            PHYSICAL
        }
    }
}
