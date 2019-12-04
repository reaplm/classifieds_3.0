using Classifieds.Domain.Data;
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
        public int RemoveAllPictures(long id)
        {
            return advertRepo.RemoveAllPictures(id);
        }
        /// <summary>
        /// Count all registered users
        /// </summary>
        /// <returns>Number of registered users</returns>
        public int CountAllAdverts()
        {
            return advertRepo.CountAllAdverts();
        }
        public List<CountPercentSummary> AdvertCountByStatus()
        {
            return advertRepo.AdvertCountByStatus();
        }
        public List<CountPercentSummary> AdvertCountByLocation()
        {
            return advertRepo.AdvertCountByLocation();
        }
    }
}
