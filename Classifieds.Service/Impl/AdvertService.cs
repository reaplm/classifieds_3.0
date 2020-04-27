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
        public int CountAdverts(string status)
        {
            return advertRepo.CountAdverts(status);
        }
        public int CountAdvertsByUser(long id)
        {
            return advertRepo.CountAdvertsByUser(id);
        }
        public int CountAdvertsByUserByStatus(long id, string status)
        {
            return advertRepo.CountAdvertsByUserByStatus(id, status);
        }
        /// <summary>
        /// Fetch summary list of adverts grouped by advert status
        /// </summary>
        /// <returns>List containing count and percentage of adverts per status</returns>
        public List<CountPercentSummary> AdvertCountByStatus()
        {
            return advertRepo.AdvertCountByStatus();
        }
       /// <summary>
       /// Summary of adverts per status for the user with ID id
       /// </summary>
       /// <param name="id">PK/ID of user</param>
       /// <returns>Summary list containing count and percentage of adverts per status</returns>
        public List<CountPercentSummary> AdvertCountByStatusByUser(long id)
        {
            return advertRepo.AdvertCountByStatusByUser(id);
        }
        /// <summary>
        /// Fetch summary list of adverts grouped by location
        /// </summary>
        /// <returns>List containing count and percentage of adverts in each location</returns>
        public List<CountPercentSummary> AdvertCountByLocation()
        {
            return advertRepo.AdvertCountByLocation();
        }
        /// <summary>
        /// Fetch summary list of adverts grouped by category
        /// </summary>
        /// <returns>List containing count and percentage of adverts in each category</returns>
        public List<CountPercentSummary> AdvertCountByCategory()
        {
            return advertRepo.AdvertCountByCategory();
        }
    }
}
