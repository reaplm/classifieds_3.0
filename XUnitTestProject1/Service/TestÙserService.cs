using Classifieds.Repository;
using Classifieds.Service;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Moq;
using Classifieds.Service.Impl;

namespace Classifieds.XUnitTest.Service
{
    public class TestÙserService
    {
        private ApplicationContext mockContext;

        public TestÙserService()
        {
            InitContext();
        }
        [Fact]
        public void TestAuthenticateUser()
        {
            var mockRepo = new Mock<IUserRepo>();
            mockRepo.Setup(x => x.authenticateUser(It.IsAny<String>(), It.IsAny<String>()))
                .Returns(new Domain.Model.User { ID = 5, Email = "user@email.com" });

            var service = new UserService(mockRepo.Object);

            var result = service.authenticateUser(It.IsAny<String>(), It.IsAny<String>());

            Assert.Equal(5, result.ID);
            Assert.Equal("user@email.com", result.Email);
        }
        [Fact]
        public void TestGetEncryptedPassword()
        {
            var mockRepo = new Mock<IUserRepo>();
            var service = new UserService(mockRepo.Object);

            var result = service.getEncryptedPassword("password2");

            Assert.Equal("6cb75f652a9b52798eb6cf2201057c73", result);
        }
        private void InitContext()
        {
            var builder = new DbContextOptionsBuilder<ApplicationContext>()
                .UseInMemoryDatabase("TestDB");
            var context = new ApplicationContext(builder.Options);

      
        }
    }
}
