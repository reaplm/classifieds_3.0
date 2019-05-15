using Classifieds.Domain.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Classifieds.Repository.Impl
{
    public class UserDetailRepo : GenericRepo<UserDetail>, IUserDetailRepo
    {
        private ApplicationContext context;

        public UserDetailRepo(ApplicationContext context) : base(context)
        {
            this.context = context;
        }
        /// <summary>
        /// Authenticate a user
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns>Authenticated user</returns>
        public User AuthenticateUser(String email, String password) 
        {
            try
            {
               User user = context.Users
                .Where(x => x.Email == email && x.Password == password)
                .Include(x => x.UserDetail)
                .SingleOrDefault();

                return user;
            }
            catch(InvalidOperationException ex)
            {
                throw;
            }


        }
    }
}
