using Classifieds.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Microsoft.EntityFrameworkCore;
using Classifieds.Domain.Model;
using Classifieds.Repository.Impl;

namespace Classifieds.XUnitTest.Repository
{
    public class TestUserRepo : IDisposable
    {
        private ApplicationContext mockContext;

        public TestUserRepo()
        {
            InitContext();
        }
        /// <summary>
        /// Test authentication
        /// </summary>
        [Fact]
        public void AuthenticateUser()
        {
            var repo = new UserRepo(mockContext);
            User user = repo.AuthenticateUser("user2@email", "6cb75f652a9b52798eb6cf2201057c73");


            Assert.Equal(2, user.ID);
            Assert.Equal("user2@email", user.Email);

        }
        /// <summary>
        /// Test authentication when password and/or email are wrong
        /// </summary>
        [Fact]
        public void AuthenticateUser_InvalidOperationException()
        {
            var repo = new UserRepo(mockContext);

            Assert.Throws<InvalidOperationException>(() =>
                repo.AuthenticateUser("my@email", "7c6a180b36896a0a8c02787eeafb0e4c"));

        }
        /// <summary>
        /// Initialize context
        /// 
        /// Hash: 7c6a180b36896a0a8c02787eeafb0e4c
        /// String: password1
        /// 
        /// Hash: 6cb75f652a9b52798eb6cf2201057c73
        /// String: password2
        /// </summary>
        private void InitContext()
        {
            var builder = new DbContextOptionsBuilder<ApplicationContext>()
                .UseInMemoryDatabase("TestDB");

            var context = new ApplicationContext(builder.Options);

            List<User> users = new List<User>
            {
                new User{ID=1,Email="my@email",Password="7c6a180b36896a0a8c02787eeafb0e4c",RegDate=new DateTime(2019,1,15)},
                new User{ID=2,Email="user2@email",Password="6cb75f652a9b52798eb6cf2201057c73",RegDate=new DateTime(2018,10,2)},
                new User{ID=3,Email="my@email",Password="7c6a180b36896a0a8c02787eeafb0e4c",RegDate=new DateTime(2018,2,22)}
            };

            List<UserDetail> userDetails = new List<UserDetail>
            {
                new UserDetail{ID=1,FirstName="fName1",LastName="lname1",UserID=1},
                new UserDetail{ID=2,FirstName="fName2",LastName="lname2",UserID=2},
                new UserDetail{ID=3,FirstName="fName3",LastName="lname3",UserID=3}
            };

            context.Users.AddRange(users);
            context.UserDetails.AddRange(userDetails);
            int changed = context.SaveChanges();
            mockContext = context;

        }
        /// <summary>
        /// Clear db for next test
        /// </summary>
        public void Dispose()
        {
            mockContext.Users.RemoveRange(mockContext.Users);
            mockContext.UserDetails.RemoveRange(mockContext.UserDetails);
            int changed = mockContext.SaveChanges();
        }
    }
}

