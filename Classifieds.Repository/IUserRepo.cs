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
        //new User Create(User user);
        int ActivateAccount(long id, string token);
        User ValidateEmailAddress(string email);
        bool UpdateResetCode(long id, string code);
        int CountAllUsers();
    }
}
