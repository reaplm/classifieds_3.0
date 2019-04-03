using Classifieds.Domain.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Classifieds.Repository
{
    public interface IUserRepo : IGenericRepo<User>
    {
        User authenticateUser(String email, String password);
    }
}
