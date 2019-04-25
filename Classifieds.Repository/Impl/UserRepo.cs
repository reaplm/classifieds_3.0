using Classifieds.Domain.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Classifieds.Repository.Impl
{
    public class UserRepo : GenericRepo<User>, IUserRepo
    {
        private ApplicationContext context;

        public UserRepo(ApplicationContext context) : base(context)
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
