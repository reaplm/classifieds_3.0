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
        private Mock<IUserService> mockUserService;
        private IMapper mapper;

        public TestLoginController()
        {
            Initialize();
            mockUserService = new Mock<IUserService>(MockBehavior.Strict);

        }
        /// <summary>
        /// Test for: public IActionResult Index(String ReturnUrl)
        /// Url to redirect to after login
        /// </summary>
        [Fact]
        public void Index()
        {
            var controller = new LoginController(mockUserService.Object, mapper);
            String returnUrl = "/Classifieds/Create";
            var result = controller.Index(returnUrl) as ViewResult;
            
            Assert.Null(result.ViewName);
            Assert.Equal("/Classifieds/Create", result.ViewData["ReturnUrl"]);
        }
        /// <summary>
        /// Test for: public async Task<IActionResult> Login(UserViewModel user)
        /// Test POST when ModelState.IsValid = false
        /// </summary>
        [Fact]
        public void Login_ModelStateIsInvalid()
        {

            mockUserService.Setup(m => m.authenticateUser(It.IsAny<String>(), It.IsAny<String>()))
               .Returns(It.IsAny<User>());

            var controller = new LoginController(mockUserService.Object, mapper);
            controller.ModelState.AddModelError("email", "email is invalid");

            UserViewModel user = new UserViewModel
            {
                ID = 1,
                Email = "my@email",
                Password = "Pass1"
            };

            var result = controller.Login(user,null).Result as ViewResult;
            var model = result.Model;

            Assert.Equal("Index", result.ViewName);
        }
        /// <summary>
        /// Test for: public async Task<IActionResult> Login(UserViewModel user)
        /// Test POST when authenticateUser(String, String) return null
        /// </summary>
        [Fact]
        public void Login_AuntheticateUserIsFalse()
        {
            User user = new User
            {
                ID = 1,
                Email = "my@email",
                Password = "Pass1"
            };

            mockUserService.Setup(m => m.authenticateUser(It.IsAny<String>(), It.IsAny<String>()))
               .Returns((User)null);

            var controller = new LoginController(mockUserService.Object, mapper);

            UserViewModel userModel = new UserViewModel
            {
                ID = 1,
                Email = "my@email",
                Password = "Pass1"
            };

            var result = controller.Login(userModel, null).Result as ViewResult;
            var model = result.Model;

            Assert.Equal("Index", result.ViewName);

        }
        /// <summary>
        /// Test for: public async Task<IActionResult> Login(UserViewModel user)
        /// login is successful, ReturnUrl = null
        /// </summary>
        [Fact]
        public void Login_SuccessfulReturnUrlNull()
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

            mockUserService.Setup(m => m.authenticateUser(It.IsAny<String>(), It.IsAny<String>()))
               .Returns(user);

            var urlHelper = new Mock<IUrlHelper>();
            urlHelper.Setup(m => m.IsLocalUrl(It.IsAny<string>())).Returns(false);

            var controller = new LoginController(mockUserService.Object, mapper);
            controller.Url = urlHelper.Object;
            controller.ControllerContext = new ControllerContext();
            controller.ControllerContext.HttpContext = new DefaultHttpContext()
            {
                RequestServices = ServiceProviderMock()
            };

            var result = controller.Login(userViewModel, null).Result;
            var redirect = (RedirectToActionResult)result;

            Assert.Equal("Index", redirect.ActionName);
            Assert.Equal("Home", redirect.ControllerName);
        }
        /// <summary>
        /// Test for: public async Task<IActionResult> Login(UserViewModel user)
        /// login is successful ReturnUrl is not null
        /// </summary>
        [Fact]
        public void Login_SuccessfulReturnUrlNotNull()
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

            mockUserService.Setup(m => m.authenticateUser(It.IsAny<String>(), It.IsAny<String>()))
               .Returns(user);

            var mockUrlHelper = new Mock<IUrlHelper>(MockBehavior.Strict);
            mockUrlHelper.Setup(m => m.IsLocalUrl(It.IsAny<string>()))
                .Returns(true);

            var controller = new LoginController(mockUserService.Object, mapper);
            controller.Url = mockUrlHelper.Object;
            controller.ControllerContext = new ControllerContext();
            controller.ControllerContext.HttpContext = new DefaultHttpContext()
            {
                RequestServices = ServiceProviderMock()
            };

            string returnUrl = "/Classifieds/Create";
            var result = controller.Login(userViewModel, returnUrl).Result;
            var redirect = (RedirectResult)result;

            Assert.Equal("/Classifieds/Create", redirect.Url);
        }
        /// <summary>
        /// Test for: public async Task<IActionResult> SignOut()
        /// </summary>
        [Fact]
        public void SignOut()
        {
            var controller = new LoginController(mockUserService.Object, mapper);

            controller.ControllerContext = new ControllerContext();
            controller.ControllerContext.HttpContext = new DefaultHttpContext()
            {
                RequestServices = ServiceProviderMock()
            };

            var result = controller.SignOut().Result;
            var redirect = (RedirectToActionResult)result;


            Assert.Equal("Index", redirect.ActionName);
            Assert.Equal("Home", redirect.ControllerName);
        }
        /// <summary>
        /// Initialize AutoMapper
        /// </summary>
        private void Initialize()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<UserViewModel, User>();
            });

            mapper = config.CreateMapper();
        }
        /// <summary>
        /// Setup authentication to mock SignInAsync
        /// </summary>
        /// <returns></returns>
        private IServiceProvider ServiceProviderMock()
        {

            var authServiceMock = new Mock<IAuthenticationService>();
            authServiceMock.Setup(m => m.SignInAsync(It.IsAny<HttpContext>(),
                It.IsAny<String>(), It.IsAny<ClaimsPrincipal>(),
                It.IsAny<AuthenticationProperties>()))
                .Returns(Task.FromResult((object)null));

            var urlHelperFactory = new Mock<IUrlHelperFactory>();
            var urlHelper = new Mock<IUrlHelper>();
            var dataDictionaryFactory = new Mock<ITempDataDictionaryFactory>();
            urlHelper.Setup(m => m.IsLocalUrl(It.IsAny<string>())).Returns(true);
            //urlHelper.Setup(m => m(It.IsAny<string>())

var serviceProviderMock = new Mock<IServiceProvider>();
            serviceProviderMock.Setup(m => m.GetService(typeof(IAuthenticationService)))
                .Returns(authServiceMock.Object);
            serviceProviderMock.Setup(m => m.GetService(typeof(IUrlHelperFactory)))
                .Returns(urlHelperFactory.Object);
            serviceProviderMock.Setup(m => m.GetService(typeof(IUrlHelper)))
                .Returns(urlHelper.Object);
            serviceProviderMock.Setup(m => m.GetService(typeof(ITempDataDictionaryFactory)))
                .Returns(dataDictionaryFactory.Object);

            return serviceProviderMock.Object;
        }


    }

}

    

