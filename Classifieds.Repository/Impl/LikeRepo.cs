using Classifieds.Domain.Model;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Classifieds.Repository.Impl
{
    public class LikeRepo : ILikeRepo
    {
        private ApplicationContext context;

        public LikeRepo(ApplicationContext context)
        {
            this.context = context;
        }
        /// <summary>
        /// Find like by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Like Find(long id)
        {
            return context.Likes.Where(x => x.ID == id).SingleOrDefault();
        }
        /// <summary>
        /// Fetch all likes for all users
        /// </summary>
        /// <returns>List of likes</returns>
        public IEnumerable<Like> FindAll()
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Fetch all likes by userId include advert details
        /// </summary>
        /// <param name="id">user id</param>
        /// <returns>List of likes</returns>
        public IEnumerable<Like> FindByUser(long id)
        {
            return context.Likes
                .Where(x => x.UserID == id)
                .Include(x => x.Advert)
                .Include(x => x.Advert.Detail)
                .ToList();
        }
        public int CountLikesByUser(long id)
        {
            return context.Likes.Count(x => x.UserID == id);
        }
    }
}
