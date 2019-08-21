using AutoMapper;
using Classifieds.Domain.Model;
using Classifieds.Service;
using Classifieds.Web.Controllers;
using Classifieds.Web.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Classifieds.XUnitTest.Controller
{
    public class TestRegistrationController
    {
        private IMapper mapper;
        private Mock<IUserService> mockUserService;

        public TestRegistrationController()
        {
            mockUserService = new Mock<IUserService>();
            Initialize();
        }
        [Fact]
        public void Index_GET()
        {
            var controller = new RegistrationController(mapper, mockUserService.Object);
            var result = controller.Index() as ViewResult;

            Assert.Equal("/Registration/ConfirmRegistration/", result.ViewData["ReturnUrl"]);
        }
        [Fact]
        public void Index_ModelStateIsValid_POST()
        {
            var model = GetData();

            mockUserService.Setup(m => m.CreateVerificationToken(It.IsAny<long>(),
                It.IsAny<string>())).Returns(true);
            mockUserService.Setup(m => m.SendVerificationEmailAsync(It.IsAny<string>(),
                It.IsAny<string>(), It.IsAny<string>())).Returns(Task.CompletedTask);
            mockUserService.Setup(m => m.Create(It.IsAny<User>()))
            .Returns(mapper.Map<User>(model.User));

            
            var controller = new RegistrationController(mapper, mockUserService.Object);
            controller.ControllerContext.HttpContext = new DefaultHttpContext();
            controller.Url = new Mock<IUrlHelper>().Object;

            var result = controller.Index(model, "/Registration/ConfirmRegistration/") as RedirectResult;

            Assert.Equal("/Registration/ConfirmRegistration/", result.Url);
        }
        [Fact]
        public void Index_ModelStateIsInvalid_POST()
        {
            var model = GetData();
            var controller = new RegistrationController(mapper, mockUserService.Object);
            controller.ModelState.AddModelError("Password", "Password is required");
            var result = controller.Index(model, "/Registration/Confirm/") as ViewResult;
            var resultModel = result.Model as RegistrationViewModel;

            Assert.Equal("pearl19", resultModel.Password);
            Assert.Equal("pearl19", resultModel.ConfirmPassword );
            Assert.True(resultModel.AcceptTerms);
        }
        /// <summary>
        /// Test for {public IActionResult ConfirmRegistration(long id, string token)}
        /// </summary>
        [Fact]
        public void ConfirmRegistration_TokenIsNull()
        {
            var controller = new RegistrationController(mapper, mockUserService.Object);
            var result = controller.ConfirmRegistration(1, null) as ViewResult;

            Assert.False((bool)result.ViewData["IsActivated"]);
        }
        /// <summary>
        /// Test for {public IActionResult ConfirmRegistration(long id, string token)}
        /// </summary>
        [Fact]
        public void ConfirmRegistration_TokenIsNotNull()
        {
            mockUserService.Setup(m => m.ActivateAccount(It.IsAny<long>(), It.IsAny<string>()))
                .Returns(1);

            var controller = new RegistrationController(mapper, mockUserService.Object);
            var result = controller.ConfirmRegistration(1, "cd-2a-hk") as ViewResult;

            Assert.True((bool)result.ViewData["IsActivated"]);
            Assert.False((bool)result.ViewData["Error"]);
        }
        /// <summary>
        /// Test for {public IActionResult ConfirmRegistration(long id, string token)}
        /// </summary>
        [Fact]
        public void ConfirmRegistration_ChangedIsLessThanZero()
        {

            mockUserService.Setup(m => m.ActivateAccount(It.IsAny<long>(), It.IsAny<string>()))
                .Returns(0);

            var controller = new RegistrationController(mapper, mockUserService.Object);
            var result = controller.ConfirmRegistration(1, "cd-2a-hk") as ViewResult;

            Assert.True((bool)result.ViewData["IsActivated"]);
            Assert.True((bool)result.ViewData["Error"]);

        }
        private RegistrationViewModel GetData()
        {
            UserDetailViewModel detail = new UserDetailViewModel
            {
                FirstName = "Pearl",
                LastName = "Molefe"
            };
            UserViewModel user = new UserViewModel
            {
                UserDetail = detail,
                Email = "pmolefe@gmail.com"

            };
            RegistrationViewModel registration = new RegistrationViewModel
            {
                User = user,
                Password = "pearl19",
                ConfirmPassword = "pearl19",
                AcceptTerms = true
            };
            return registration;

        }
        private void Initialize()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<User, UserViewModel>();
            });

            mapper = config.CreateMapper();
        }
    }
}
