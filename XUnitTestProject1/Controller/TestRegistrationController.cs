using AutoMapper;
using Classifieds.Domain.Model;
using Classifieds.Service;
using Classifieds.Web.Controllers;
using Classifieds.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
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

            Assert.Equal("/Login/", result.ViewData["ReturnUrl"]);
        }
        [Fact]
        public void Index_ModelStateIsValid_POST()
        {
            var model = GetData();
            var controller = new RegistrationController(mapper, mockUserService.Object);

            var result = controller.Index(model, "/Login/") as RedirectResult;

            Assert.Equal("/Login/", result.Url);
        }
        [Fact]
        public void Index_ModelStateIsInvalid_POST()
        {
            var model = GetData();
            var controller = new RegistrationController(mapper, mockUserService.Object);
            controller.ModelState.AddModelError("Password", "Password is required");
            var result = controller.Index(model, "/Login/") as ViewResult;
            var resultModel = result.Model as RegistrationViewModel;

            Assert.Equal("pearl19", resultModel.Password);
            Assert.Equal("pearl19", resultModel.ConfirmPassword );
            Assert.True(resultModel.AcceptTerms);
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
