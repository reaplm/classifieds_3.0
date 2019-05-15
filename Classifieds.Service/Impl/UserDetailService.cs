using Classifieds.Domain.Model;
using Classifieds.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace Classifieds.Service.Impl
{
    public class UserDetailService : GenericService<UserDetail>,IUserDetailService
    {
        private IUserDetailRepo userDetailRepo;

        public UserDetailService(IUserDetailRepo userDetailRepo) : base(userDetailRepo)
        {
            this.userDetailRepo = userDetailRepo;
        }
    }
}
