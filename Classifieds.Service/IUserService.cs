using Classifieds.Domain.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Classifieds.Service
{
    public interface IUserService : IGenericService<User>
    {
        User authenticateUser(String email, String password);
        String getEncryptedPassword(String password);
    }
}
