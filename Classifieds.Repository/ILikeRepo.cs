using Classifieds.Domain.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Classifieds.Repository
{
    public interface ILikeRepo
    {
        IEnumerable<Like> FindAll();
        IEnumerable<Like> FindByUser(long id);
        Like Find(long id);
        int CountLikesByUser(long id);
    }
}
