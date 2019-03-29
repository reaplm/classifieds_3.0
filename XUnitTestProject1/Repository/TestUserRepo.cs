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
    public class TestUserRepo
    {
        private ApplicationContext mockContext;

        public TestUserRepo()
        {
            initContext();
        }

        [Fact]
        public void testFindAll()
        {
            var repo = new UserRepo(mockContext);
            var users = repo.findAll() as IEnumerable<User>;

            Assert.Equal(3, users.Count());
            Assert.NotNull(users.ElementAt(0).UserDetail);
            Assert.NotNull(users.ElementAt(1).UserDetail);
            Assert.NotNull(users.ElementAt(2).UserDetail);
        }
         private void initContext()
        {
            var builder = new DbContextOptionsBuilder<ApplicationContext>()
                .UseInMemoryDatabase("TestDB");

            var context = new ApplicationContext(builder.Options);

            List<User> users = new List<User>
            {
                new User{ID=1,Email="my@email",Password="Pass1",RegDate=new DateTime(2019,1,15)},
                new User{ID=2,Email="my@email",Password="Pass2",RegDate=new DateTime(2018,10,2)},
                new User{ID=3,Email="my@email",Password="Pass3",RegDate=new DateTime(2018,2,22)}
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
    }
}
