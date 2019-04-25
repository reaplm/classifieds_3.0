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
        /// <summary>
        /// Test { public User AuthenticateUser(String email, String password) }
        /// </summary>
        [Fact]
        public void AuthenticateUser()
        {
            var mockRepo = new Mock<IUserRepo>();
            mockRepo.Setup(x => x.AuthenticateUser(It.IsAny<String>(), It.IsAny<String>()))
                .Returns(new Domain.Model.User { ID = 5, Email = "user@email.com" });

            var service = new UserService(mockRepo.Object);

            var result = service.AuthenticateUser(It.IsAny<String>(), It.IsAny<String>());

            Assert.Equal(5, result.ID);
            Assert.Equal("user@email.com", result.Email);
        }
        /// <summary>
        /// Test { public String GetEncryptedPassword(String password) }
        /// </summary>
        [Fact]
        public void GetEncryptedPassword()
        {
            var mockRepo = new Mock<IUserRepo>();
            var service = new UserService(mockRepo.Object);

            var result = service.GetEncryptedPassword("password2");

            Assert.Equal("6cb75f652a9b52798eb6cf2201057c73", result);
        }
    }
}
