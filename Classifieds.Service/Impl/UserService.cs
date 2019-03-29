using Classifieds.Domain.Model;
using Classifieds.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace Classifieds.Service.Impl
{
    public class UserService : GenericService<User>, IUserService
    {
        private IUserRepo userRepository;

        public UserService(IUserRepo userRepository) : base(userRepository)
        {
            this.userRepository = userRepository;
        }
    }
}
