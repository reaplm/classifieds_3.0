using Classifieds.Repository;
using System;
using System.Collections.Generic;
using System.Text;


namespace Classifieds.Service
{
    public interface IGenericService<T> : IGenericRepo<T> where T : class
    {
    }
}
