using Classifieds.Domain.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Classifieds.Service
{
    public interface IUserService : IGenericService<User>
    {
        User AuthenticateUser(String email, String password);
        String GetEncryptedPassword(String password);
    }
}
