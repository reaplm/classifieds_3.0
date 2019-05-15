using Classifieds.Domain.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Classifieds.Repository.Impl
{
    

    public class AddressRepo : GenericRepo<Address>,IAddressRepo
    {
        private ApplicationContext appContext;

        public AddressRepo(ApplicationContext appContext) : base(appContext)
        {
            this.appContext = appContext;
        }
    }
}
