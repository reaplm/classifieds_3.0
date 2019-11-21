using Classifieds.Domain.Model;
using Classifieds.Service;
using Classifieds.Web.Controllers;
using Classifieds.Web.Models;
using Microsoft.AspNetCore.Http;
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
            User user = new User { ID = 15, Email = "p@gmail.com" };
            mockUserService.Setup(m => m.ValidateEmailAddress(It.IsAny<string>()))
                .Returns(user);
            mockUserService.Setup(m => m.RandomCodeGenerator())
                .Returns(It.IsAny<string>());
            mockUserService.Setup(m => m.SendEmailAsync(It.IsAny<string>(),
                It.IsAny<string>(), It.IsAny<string>())).Returns(Task.CompletedTask);


            var controller = new PasswordController(mockUserService.Object);
            controller.ControllerContext.HttpContext = new DefaultHttpContext();
            controller.Url = controller.Url = new Mock<IUrlHelper>().Object;
            var result = controller.Index("p@gmail.com") as RedirectToActionResult;
            object id;
            result.RouteValues.TryGetValue("id", out id);
            Assert.Equal("Reset", result.ActionName);
            Assert.Equal(15, (long)id);
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
        [Fact]
        public void Reset_GET()
        {
            User user = new User { ID = 15, Email = "p@gmail.com" };
            mockUserService.Setup(m => m.Find(It.IsAny<long>())).Returns(user);
            var controller = new PasswordController(mockUserService.Object);
            var result = controller.Reset(15) as ViewResult;
            var model = result.Model as PasswordResetViewModel;

            Assert.Equal(15, model.ID);
            Assert.Equal("p@gmail.com", model.Email);
        }
        [Fact]
        public void Reset_ModelStateIsValid_POST()
        {
            User user = new User
            {
                ID = 15,
                Email = "p@gmail.com",
                ResetCode = "ZABGH52Y"
            };

            PasswordResetViewModel model = new PasswordResetViewModel
            {
                ID = 15,
                Email = "p@gmail.com",
                Password = "pass1",
                ResetCode = "ZABGH52Y",
                ConfirmPassword = "pass1"
            };

            mockUserService.Setup(m => m.Find(It.IsAny<long>())).Returns(user);
            mockUserService.Setup(m => m.GetEncryptedPassword(It.IsAny<string>()))
                .Returns(It.IsAny<string>());

            var controller = new PasswordController(mockUserService.Object);

            var result = controller.Reset(model) as RedirectToActionResult;

            Assert.Equal("ResetSuccess", result.ActionName);
        }
        [Fact]
        public void Reset_ModelStateIsInvalid_POST()
        {
            User user = new User
            {
                ID = 15,
                Email = "p@gmail.com",
                ResetCode = "ZABGH52Y"
            };

            PasswordResetViewModel model = new PasswordResetViewModel
            {
                ID = 15,
                Email = "p@gmail.com",
                Password = "pass1",
                ResetCode = "ZABGH52Y",
                ConfirmPassword = "pass"
            };

            mockUserService.Setup(m => m.Find(It.IsAny<long>())).Returns(user);

            var controller = new PasswordController(mockUserService.Object);
            controller.ModelState.AddModelError("ConfirmPassword", "Passwords must match");

            var result = controller.Reset(model) as ViewResult;
            var modelResult = result.Model as PasswordResetViewModel;

            Assert.Equal(15, modelResult.ID);
            Assert.Equal("p@gmail.com", modelResult.Email);
        }
        [Fact]
        public void Reset_WrongResetCode_POST()
        {
            User user = new User
            {
                ID = 15,
                Email = "p@gmail.com",
                ResetCode = "ZABGH52Y"
            };

            PasswordResetViewModel model = new PasswordResetViewModel
            {
                ID = 15,
                Email = "p@gmail.com",
                Password = "pass1",
                ResetCode = "ZABGH52X",
                ConfirmPassword = "pass"
            };

            mockUserService.Setup(m => m.Find(It.IsAny<long>())).Returns(user);

            var controller = new PasswordController(mockUserService.Object);
            var result = controller.Reset(model) as ViewResult;
            var modelResult = result.Model as PasswordResetViewModel;

            Assert.Equal(15, modelResult.ID);
            Assert.Equal("p@gmail.com", modelResult.Email);
        }
    }
}
