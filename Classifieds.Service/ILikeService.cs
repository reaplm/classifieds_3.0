using Classifieds.Domain.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Classifieds.Service
{
    public interface ILikeService
    {
        IEnumerable<Like> FindAll();
        Like Find(long id);
        IEnumerable<Like> FindByUser(long id);
        int CountLikesByUser(long id);
    }
}
