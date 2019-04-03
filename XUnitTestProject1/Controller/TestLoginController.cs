using Classifieds.Repository.Impl;
using Classifieds.Service;
using Classifieds.Service.Impl;
using System;
using System.Collections.Generic;
using System.Text;
using Moq;
using Classifieds.Repository;
using Classifieds.Web.Controllers;
using Xunit;
using Microsoft.AspNetCore.Mvc;
using Classifieds.Web.Models;
using Classifieds.Domain.Model;
using AutoMapper;

namespace Classifieds.XUnitTest.Controller
{
    public class TestLoginController
    {
        private Mock<IUserService> mockService;
        private IMapper mapper;

        public TestLoginController()
        {
            Initialize();
            mockService = new Mock<IUserService>();
        }
        [Fact]
        public void Login_ModelState_IsInvalid()
        {
            
            mockService.Setup(m => m.authenticateUser(It.IsAny<String>(), It.IsAny<String>()))
               .Returns(It.IsAny<User>());

            var controller = new LoginController(mockService.Object, mapper);
            controller.ModelState.AddModelError("email", "email is invalid");

            UserViewModel user = new UserViewModel
            {
                ID = 1, Email = "my@email", Password = "Pass1"
            };


            //return View("Index", user);
            var result = controller.Login(user)as ViewResult;
            var model = result.Model;

            Assert.Equal("Index", result.ViewName);

        }
        [Fact]
        public void Login_Aunthetication_IsFalse()
        {
            mockService.Setup(m => m.authenticateUser(It.IsAny<String>(), It.IsAny<String>()))
               .Returns(It.IsAny<User>());

            var controller = new LoginController(mockService.Object, mapper);

            UserViewModel user = new UserViewModel
            {
                ID = 1,
                Email = "my@email",
                Password = "Pass1"
            };


            //return View("Index", user);
            var result = controller.Login(user) as ViewResult;
            var model = result.Model;

            Assert.Equal("Index", result.ViewName);

        }
        [Fact]
        public void Login_Successful()
        {
            User user = new User
            {
                ID = 1,
                Email = "my@email",
                Password = "Pass1"
            };

            UserViewModel userViewModel = new UserViewModel
            {
                ID = 1,
                Email = "my@email",
                Password = "Pass1"
            };

            var mockService = new Mock<IUserService>();
            mockService.Setup(m => m.authenticateUser(It.IsAny<String>(), It.IsAny<String>()))
                .Returns(user);

            var controller = new LoginController(mockService.Object, mapper);

            var result = (RedirectToActionResult) controller.Login(userViewModel);
    

            Assert.Equal("Index", result.ActionName);
            Assert.Equal("Home", result.ControllerName);
        }
        private void Initialize()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<UserViewModel, User>();
            });

            mapper = config.CreateMapper();
        }
    }

    
}
