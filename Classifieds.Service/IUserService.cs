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
        Task SendVerificationEmailAsync(string email, string subject, string message);
        new User Create(User user);
    }
}
