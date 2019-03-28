using Classifieds.Domain.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Classifieds.Service
{
    public interface IAdvertService : IGenericService<Advert>
    {
        IEnumerable<Advert> findByCategory(int id);
    }
}
