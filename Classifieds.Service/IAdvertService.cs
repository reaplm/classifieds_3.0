using Classifieds.Domain.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Classifieds.Service
{
    public interface IAdvertService : IGenericService<Advert>
    {
        IEnumerable<Advert> FindByCategory(int id);
        IEnumerable<Advert> FindBySubCategory(int id);
        int RemoveAllPictures(long id);
        int CountAllAdverts();
    }
}
