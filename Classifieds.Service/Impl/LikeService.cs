using Classifieds.Domain.Model;
using Classifieds.Repository;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Classifieds.Service.Impl
{
    public class LikeService : ILikeService
    {
        private ILikeRepo likeRepository;

        public LikeService(ILikeRepo likeRepository)
        {
            this.likeRepository = likeRepository;
        }
        /// <summary>
        /// Find like using id
        /// </summary>
        /// <param name="id">id of the like</param>
        /// <returns></returns>
        public Like Find(long id)
        {
            return likeRepository.Find(id);
        }
        /// <summary>
        /// Fetch all likes for all users
        /// </summary>
        /// <returns>list of likes</returns>
        public IEnumerable<Like> FindAll()
        {
            return likeRepository.FindAll();
        }
        /// <summary>
        /// Fetch all likes for given user using user id
        /// </summary>
        /// <param name="id">user id</param>
        /// <returns>List of likes</returns>
        public IEnumerable<Like> FindByUser(long id)
        {
            return likeRepository.FindByUser(id);
        }
        public int CountLikesByUser(long id)
        {
            return likeRepository.CountLikesByUser(id);
        }
    }
}
