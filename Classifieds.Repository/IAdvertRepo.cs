using Classifieds.Domain.Data;
using Classifieds.Domain.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Classifieds.Repository
{
    public interface IAdvertRepo : IGenericRepo<Advert>
    {
        IEnumerable<Advert> FindByCategory(int id);
        IEnumerable<Advert> FindBySubCategory(int id);
        int RemoveAllPictures(long id);
        int CountAdverts(string status);
        int CountAdvertsByUser(long id);
        int CountAdvertsByUserByStatus(long id, string status);
        List<CountPercentSummary> AdvertCountByStatus();
        List<CountPercentSummary> AdvertCountByStatusByUser(long id);
        List<CountPercentSummary> AdvertCountByLocation();
        List<CountPercentSummary> AdvertCountByCategory();
    }
}
