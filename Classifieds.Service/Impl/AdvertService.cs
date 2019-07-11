using Classifieds.Domain.Model;
using Classifieds.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace Classifieds.Service.Impl
{
    public class AdvertService : GenericService<Advert>, IAdvertService
    {
        private IAdvertRepo advertRepo;

        public AdvertService(IAdvertRepo advertRepo) : base(advertRepo)
        {
            this.advertRepo = advertRepo;
        }
        public IEnumerable<Advert> FindByCategory(int id)
        {
            return advertRepo.FindByCategory(id);
        }
        public IEnumerable<Advert> FindBySubCategory(int id)
        {
            return advertRepo.FindBySubCategory(id);
        }
    }
}
