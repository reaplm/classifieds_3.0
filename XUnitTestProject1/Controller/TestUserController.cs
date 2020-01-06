using AutoMapper;
using Classifieds.Domain.Model;
using Classifieds.Service;
using Classifieds.Web.Controllers;
using Classifieds.Web.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using Xunit;

namespace Classifieds.XUnitTest.Controller
{
    public class TestUserController
    {
        private Mock<IUserService> mockUserService;
        private IMapper mapper;

        public TestUserController()
        {
            mockUserService = new Mock<IUserService>();
            Initialize();
        }
        [Fact]
        public void Delete_ChangedGreaterThanZero()
        {
            mockUserService.Setup(m => m.Delete(It.IsAny<long>())).Returns(1);

            var controller = new UserController(mockUserService.Object, mapper);
            var result = controller.Delete(5) as JsonResult;

            Assert.Equal("Delete Successful!", result.Value);
        }
        [Fact]
        public void Delete_ChangedIsZero()
        {
            mockUserService.Setup(m => m.Delete(It.IsAny<long>())).Returns(0);

            var controller = new UserController(mockUserService.Object,mapper);
            var result = controller.Delete(5) as JsonResult;

            Assert.Equal("Delete Failed. Sorry.", result.Value);
        }
        /// <summary>
        /// Test for {public bool Like(long id, bool like)}
        /// Test result when like is True
        /// </summary>
        [Fact]
        public void Like_LikeIsTrue()
        {
            var mockIdentity = new GenericIdentity("User");
            mockIdentity.AddClaim(new Claim("UserId", "2"));
            var principal = new GenericPrincipal(mockIdentity, null);


            var mockHttpContext = new Mock<HttpContext>();
            mockHttpContext.Setup(m => m.User).Returns(principal);

            var user = new User { ID = 2, Email = "user@email.com" };
            mockUserService.Setup(m => m.Find(It.IsAny<long>())).Returns(user);

            var controller = new UserController(mockUserService.Object, mapper);
            controller.ControllerContext.HttpContext = mockHttpContext.Object;

            bool result = controller.Like(10, true);

            Assert.True(result);

        }
        /// <summary>
        /// Test for {public bool Like(long id, bool like)}
        /// Test result when like is False
        /// </summary>
        [Fact]
        public void Like_LikeIsFalse()
        {
            var mockIdentity = new GenericIdentity("User");
            mockIdentity.AddClaim(new Claim("UserId", "2"));
            var principal = new GenericPrincipal(mockIdentity, null);


            var mockHttpContext = new Mock<HttpContext>();
            mockHttpContext.Setup(m => m.User).Returns(principal);

            var user = new User
            {
                ID = 2,
                Email = "user@email.com",
                Likes =new List<Like> { new Like { AdvertID = 5 }, new Like { AdvertID = 10 } }
            };
            mockUserService.Setup(m => m.Find(It.IsAny<long>())).Returns(user);

            var controller = new UserController(mockUserService.Object, mapper);
            controller.ControllerContext.HttpContext = mockHttpContext.Object;

            bool result = controller.Like(10, false);

            Assert.True(result);
        }
        [Fact]
        public void Like_Exception()
        {
            var mockIdentity = new GenericIdentity("User");
            mockIdentity.AddClaim(new Claim("UserId", ""));
            var principal = new GenericPrincipal(mockIdentity, null);


            var mockHttpContext = new Mock<HttpContext>();
            mockHttpContext.Setup(m => m.User).Returns(principal);

            var user = new User
            {
                ID = 2,
                Email = "user@email.com",
                Likes = new List<Like> { new Like { AdvertID = 5 }, new Like { AdvertID = 10 } }
            };
            mockUserService.Setup(m => m.Find(It.IsAny<long>())).Returns(user);

            var controller = new UserController(mockUserService.Object, mapper);
            controller.ControllerContext.HttpContext = mockHttpContext.Object;

            bool result = controller.Like(10, false);

            Assert.False(result);
        }
        /// <summary>
        /// Initialize Mapper
        /// </summary>
        private void Initialize()
        {
            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<UserViewModel, User>();
            });

            mapper = configuration.CreateMapper();

        }
    }
}
