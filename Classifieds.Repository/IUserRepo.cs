using Classifieds.Domain.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Classifieds.Repository
{
    public interface IUserRepo : IGenericRepo<User>
    {
        User AuthenticateUser(String email, String password);
        bool CreateVerificationToken(long id, string token);
        new User Create(User user);
    }
}
