using Classifieds.Domain.Model;
using Classifieds.Repository;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Classifieds.Service.Impl
{
    public class UserService : GenericService<User>, IUserService
    {
        private IUserRepo userRepository;
        private IEmailService emailService;

        public UserService(IUserRepo userRepository, IEmailService emailService) : base(userRepository)
        {
            this.userRepository = userRepository;
            this.emailService = emailService;
        }

        public int ActivateAccount(long id, string token)
        {
            return userRepository.ActivateAccount(id, token);
        }

        /// <summary>
        /// Authentication Service
        /// </summary>
        /// <param name="email">Email</param>
        /// <param name="password">Password</param>
        /// <returns></returns>
        public User AuthenticateUser(String email, String password)
        {
            String encryptedPass = GetEncryptedPassword(password);

            return userRepository.AuthenticateUser(email, encryptedPass);
        }

        public bool CreateVerificationToken(long id, string token)
        {
            return userRepository.CreateVerificationToken(id, token);
        }

        /// <summary>
        /// Return encrypted password
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public String GetEncryptedPassword(String password)
        {
            

            if (String.IsNullOrEmpty(password))
            {
                return null;
            }
            else
            {
                MD5 mD5 = System.Security.Cryptography.MD5.Create();
                byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(password);
                byte[] hashBytes = mD5.ComputeHash(inputBytes);

                StringBuilder sb = new StringBuilder();
                for(int i =0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("x2"));
                }
                return sb.ToString();
            }
        }

        public async Task SendVerificationEmailAsync(string email, string subject, string message)
        {
            await emailService.SendEmailAsync(email, subject, message);

        }

        User IUserService.Create(User user)
        {
            return userRepository.Create(user);
        }

    }
}
