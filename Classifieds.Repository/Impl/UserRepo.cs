using Classifieds.Domain.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
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

        public override IEnumerable<User> findAll()
        {
            return context.Users.Include(x => x.UserDetail);
        }
    }
}
