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
using System.Linq.Expressions;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using Xunit;

namespace Classifieds.XUnitTest.Controller
{
    /// <summary>
    /// Test Notification controller
    /// </summary>
    public class TestNotificationController
    {
        private Mock<IUserService> mockUserService;
        private Mock<INotificationService> mockNotificationService;
        private IMapper mapper;

        public TestNotificationController()
        {
            Initialize();
            mockUserService = new Mock<IUserService>();
            mockNotificationService = new Mock<INotificationService>();
        }
        /// <summary>
        /// Test Add when add is true
        /// </summary>
        [Fact]
        public async void Add_AddIsTrue()
        {
            //Mock Claims object
            var mockIdentity = new GenericIdentity("User");
            mockIdentity.AddClaim(new Claim("UserId", "2"));
            var principal = new GenericPrincipal(mockIdentity, null);

            //Mock HttpContext
            var mockHttpContext = new Mock<HttpContext>();
            mockHttpContext.Setup(m => m.User).Returns(principal);

            var user = new User
            {
                ID = 1,
                Activated = 0,
                Email = "user@email",
                Notifications = new List<Notification>()
            };


            mockUserService.Setup(m => m.Find(It.IsAny<long>(), It.IsAny<Expression<Func<User, object>>[]>()))
                .Returns(user);

            var controller = new NotificationController(mapper, mockUserService.Object, mockNotificationService.Object);
            controller.ControllerContext.HttpContext = mockHttpContext.Object;

            var result = await controller.Add(true, 2, 1, 3) as JsonResult;

            Assert.Equal("Done!", result.Value);
        }
        /// <summary>
        /// Test Add when add is false
        /// </summary>
        [Fact]
        public async void Add_AddIsFalse()
        {
            //Mock Claims object
            var mockIdentity = new GenericIdentity("User");
            mockIdentity.AddClaim(new Claim("UserId", "2"));
            var principal = new GenericPrincipal(mockIdentity, null);

            //Mock HttpContext
            var mockHttpContext = new Mock<HttpContext>();
            mockHttpContext.Setup(m => m.User).Returns(principal);

            List<Notification> notifications = new List<Notification>
            {
                new Notification
                {
                    NotificationCatID = 1,
                    NotificationTypeID = 3,
                    DeviceID = 2,
                    UserID = 2
                },
                new Notification
                {
                    NotificationCatID = 2,
                    NotificationTypeID = 1,
                    DeviceID = 1,
                    UserID = 2
                },
            };

            var user = new User
            {
                ID = 1,
                Activated = 0,
                Email = "user@email",
                Notifications = notifications
            };

            
            mockUserService.Setup(m => m.Find(It.IsAny<long>(), It.IsAny<Expression<Func<User, object>>[]>()))
                .Returns(user);

            var controller = new NotificationController(mapper, mockUserService.Object, mockNotificationService.Object);
            controller.ControllerContext.HttpContext = mockHttpContext.Object;

            var result = await controller.Add(false, 2, 1, 3) as JsonResult;

            Assert.Equal("Done!", result.Value);
        }
        /// <summary>
        /// Test Add when there's an exception (FormatException)
        /// </summary>
        [Fact]
        public async void Add_ExceptionThrown()
        {
            //Mock Claims object
            var mockIdentity = new GenericIdentity("User");
            mockIdentity.AddClaim(new Claim("UserId", "k"));
            var principal = new GenericPrincipal(mockIdentity, null);

            //Mock HttpContext
            var mockHttpContext = new Mock<HttpContext>();
            mockHttpContext.Setup(m => m.User).Returns(principal);

            List<Notification> notifications = new List<Notification>
            {
                new Notification
                {
                    NotificationCatID = 1,
                    NotificationTypeID = 3,
                    DeviceID = 2,
                    UserID = 2
                },
                new Notification
                {
                    NotificationCatID = 2,
                    NotificationTypeID = 1,
                    DeviceID = 1,
                    UserID = 2
                },
            };

            var user = new User
            {
                ID = 1,
                Activated = 0,
                Email = "user@email",
                Notifications = notifications
            };


            mockUserService.Setup(m => m.Find(It.IsAny<long>(), It.IsAny<Expression<Func<User, object>>[]>()))
                .Returns(user);

            var controller = new NotificationController(mapper, mockUserService.Object, mockNotificationService.Object);
            controller.ControllerContext.HttpContext = mockHttpContext.Object;

            var result = await controller.Add(false, 2, 1, 3) as JsonResult;

            Assert.Equal("Failed to update notification. Sorry.", result.Value);
        }
        private void Initialize()
        {
            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Notification, NotificationViewModel>();
                cfg.CreateMap<NotificationViewModel, Notification>();
                cfg.CreateMap<User, UserViewModel>()
                .ForMember(m => m.RoleList, opts => opts.Ignore());
                cfg.CreateMap<UserViewModel, User>()
                .ForMember(m => m.ResetCode, opts => opts.Ignore());
            });

            mapper = mapperConfig.CreateMapper();
        }
    }
}
