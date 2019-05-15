using Classifieds.Domain.Model;
using Classifieds.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace Classifieds.Service.Impl
{
    public class AddressService : GenericService<Address>,IAddressService
    {
        private IAddressRepo addressRepo;

        public AddressService(IAddressRepo addressRepo) : base(addressRepo)
        {
            this.addressRepo = addressRepo;
        }
    }
}
