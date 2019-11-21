using Classifieds.Domain.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Classifieds.Service
{
    public interface IUserService : IGenericService<User>
    {
        User AuthenticateUser(String email, String password);
        String GetEncryptedPassword(String password);
        bool CreateVerificationToken(long id, string token);
        Task SendEmailAsync(string email, string subject, string message);
        new User Create(User user);
        int ActivateAccount(long id, string token);
        User ValidateEmailAddress(string email);
        string RandomCodeGenerator();
    }
}
