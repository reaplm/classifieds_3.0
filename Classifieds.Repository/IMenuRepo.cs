using Classifieds.Domain.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Classifieds.Repository
{
    public interface IMenuRepo : IGenericRepo<Menu>
    {
        IEnumerable<Menu> FindByType(String[] types);
    }
}
