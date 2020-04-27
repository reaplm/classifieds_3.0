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

        public async Task SendEmailAsync(string email, string subject, string message)
        {
            await emailService.SendEmailAsync(email, subject, message);

        }

        /*public User CreateEntity(User user)
        {
            return userRepository.CreateEntity(user);
        }*/
        public User ValidateEmailAddress(string email)
        {
            return userRepository.ValidateEmailAddress(email);
        }
        public bool UpdateResetCode(long id, string code)
        {
            return userRepository.UpdateResetCode(id, code);
        }
        public string RandomCodeGenerator()
        {
            StringBuilder sb = new StringBuilder();
            int codeLen = 8;
            Random r = new Random();

            for (int i = 0; i < codeLen; i++)
            {
                //get ascii character
                sb.Append((char)r.Next(48, 91));
            }
            return sb.ToString();
        }
        public int CountAllUsers()
        {
            return userRepository.CountAllUsers();
        }
        public int CountVerifiedUsers()
        {
            return userRepository.CountVerifiedUsers();
        }

        User IUserService.Create(User user)
        {
            throw new NotImplementedException();
        }
    }
}
