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
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

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
                ID = 1,
                Email = "my@email",
                Password = "Pass1"
            };


            //return View("Index", user);
            var result = controller.Login(user).Result as ViewResult;
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
            var result = controller.Login(user).Result as ViewResult;
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

            mockService.Setup(m => m.authenticateUser(It.IsAny<String>(), It.IsAny<String>()))
               .Returns(user);

            var controller = new LoginController(mockService.Object, mapper);

            controller.ControllerContext = new ControllerContext();
            controller.ControllerContext.HttpContext = new DefaultHttpContext()
            {
                RequestServices = ServiceProviderMock()
            };

            var result = controller.Login(userViewModel).Result;
            var redirect = (RedirectToActionResult)result;


            Assert.Equal("Index", redirect.ActionName);
            Assert.Equal("Home", redirect.ControllerName);
        }
        private void Initialize()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<UserViewModel, User>();
            });

            mapper = config.CreateMapper();
        }
        private IServiceProvider ServiceProviderMock()
        {

            var authServiceMock = new Mock<IAuthenticationService>();
            authServiceMock.Setup(m => m.SignInAsync(It.IsAny<HttpContext>(),
                It.IsAny<String>(), It.IsAny<ClaimsPrincipal>(),
                It.IsAny<AuthenticationProperties>()))
                .Returns(Task.FromResult((object)null));

            var urlHelperFactory = new Mock<IUrlHelperFactory>();

            var dataDictionaryFactory = new Mock<ITempDataDictionaryFactory>();

            var serviceProviderMock = new Mock<IServiceProvider>();
            serviceProviderMock.Setup(m => m.GetService(typeof(IAuthenticationService)))
                .Returns(authServiceMock.Object);
            serviceProviderMock.Setup(m => m.GetService(typeof(IUrlHelperFactory)))
                .Returns(urlHelperFactory.Object);
            serviceProviderMock.Setup(m => m.GetService(typeof(ITempDataDictionaryFactory)))
                .Returns(dataDictionaryFactory.Object);

            return serviceProviderMock.Object;
        }


    }

}

    

