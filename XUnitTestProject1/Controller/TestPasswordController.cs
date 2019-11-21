using Classifieds.Domain.Model;
using Classifieds.Service;
using Classifieds.Web.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Classifieds.XUnitTest.Controller
{
    public class TestPasswordController
    {
        private Mock<IUserService> mockUserService;

        public TestPasswordController()
        {
            mockUserService = new Mock<IUserService>();
        }
        [Fact]
        public void Index_UserNotNull_POST()
        {
            User user = new User { ID = 1, Email = "p@gmail.com" };
            mockUserService.Setup(m => m.ValidateEmailAddress(It.IsAny<string>()))
                .Returns(user);
            mockUserService.Setup(m => m.RandomCodeGenerator())
                .Returns(It.IsAny<string>());
            mockUserService.Setup(m => m.SendEmailAsync(It.IsAny<string>(),
                It.IsAny<string>(), It.IsAny<string>())).Returns(Task.CompletedTask);

            var controller = new PasswordController(mockUserService.Object);
            var result = controller.Index("p@gmail.com") as RedirectToActionResult;

            Assert.Equal("ResetSuccess", result.ActionName);

        }
        [Fact]
        public void Index_UserIsNull_POST()
        {
            User user = null;
            mockUserService.Setup(m => m.ValidateEmailAddress(It.IsAny<string>()))
                .Returns(user);

            var controller = new PasswordController(mockUserService.Object);
            var result = controller.Index("p@gmail.com") as ViewResult;

            string resultMsg = "The email address you entered does not exist. " +
                "You can either try another email address, or log in through " +
                "<a>Facebook or Google</a> - if your account is connected.";

            Assert.Equal(resultMsg, result.ViewData["Message"]);
            Assert.True((bool)result.ViewData["Error"]);
        }
    }
}
