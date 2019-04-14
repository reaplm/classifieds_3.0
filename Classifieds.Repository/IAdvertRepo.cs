using Classifieds.Domain.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Classifieds.Repository
{
    public interface IAdvertRepo : IGenericRepo<Advert>
    {
        IEnumerable<Advert> findByCategory(int id);
        Advert Find(long id);
    }
}
