﻿using Classifieds.Repository;
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

        [Fact]
        public void TestFindAll()
        {
            var repo = new UserRepo(mockContext);
            var users = repo.findAll() as IEnumerable<User>;

            Assert.Equal(3, users.Count());
            Assert.NotNull(users.ElementAt(0).UserDetail);
            Assert.NotNull(users.ElementAt(1).UserDetail);
            Assert.NotNull(users.ElementAt(2).UserDetail);
        }
         [Fact]
         public void TestAuthenticateUser()
         {
             var repo = new UserRepo(mockContext);
             User user = repo.authenticateUser("user2@email", "6cb75f652a9b52798eb6cf2201057c73");


             Assert.Equal(2, user.ID);
            Assert.Equal("user2@email", user.Email);

         }
        [Fact]
        public void TestAuthenticateUser_InvalidOperationException()
        {
            var repo = new UserRepo(mockContext);

            Assert.Throws<InvalidOperationException>(() => 
                repo.authenticateUser("my@email", "7c6a180b36896a0a8c02787eeafb0e4c"));
            
        }
        //Hash: 7c6a180b36896a0a8c02787eeafb0e4c
        //String: password1

        //Hash: 6cb75f652a9b52798eb6cf2201057c73
        //String: password2

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

        public void Dispose()
        {
            mockContext.Users.RemoveRange(mockContext.Users);
            mockContext.UserDetails.RemoveRange(mockContext.UserDetails);
            int changed = mockContext.SaveChanges();
        }
    }
}
