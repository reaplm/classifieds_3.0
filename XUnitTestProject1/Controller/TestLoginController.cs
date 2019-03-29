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

namespace Classifieds.XUnitTest.Controller
{
    public class TestLoginController
    {
        private IUserService userService;

        public TestLoginController()
        {
            userService = new Mock<IUserService>().Object;
        }
        [Fact]
        public void textIndex()
        {
            var mockService = new Mock<IUserService>();
            var controller = new LoginController(mockService.Object);
            ViewResult result = controller.Index()as ViewResult;

        }
    }
}
