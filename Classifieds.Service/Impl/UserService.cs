using Classifieds.Domain.Model;
using Classifieds.Repository;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
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
    }
}
