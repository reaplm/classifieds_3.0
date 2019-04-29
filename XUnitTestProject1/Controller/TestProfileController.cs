using AutoMapper;
using Classifieds.Domain.Model;
using Classifieds.Service;
using Classifieds.Web.Controllers;
using Classifieds.Web.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using System;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using Xunit;

namespace Classifieds.XUnitTest.Controller
{
    public class TestProfileController
    {
        private Mock<IUserService> mockUserService;
        private Mock<ClaimsPrincipal> mockClaimsPrincipal;
        private IMapper mapper;

        public TestProfileController()
        {
            ConfigMapper();
            mockUserService = new Mock<IUserService>();
            mockClaimsPrincipal = new Mock<ClaimsPrincipal>();
        }
        [Fact]
        public void Index()
        {
            User user = new User
            {
                ID = 1,
                Email = "my@email",
                Password = "Pass1"
            };

            mockUserService.Setup(m => m.Find(It.IsAny<long>(),
                It.IsAny<Expression<Func<User, Object>>[]>()))
                .Returns(user);

            var mockIdentity = new GenericIdentity("User");
            mockIdentity.AddClaim(new Claim("UserId", "1"));
            var principal = new GenericPrincipal(mockIdentity, null);


            var mockHttpContext = new Mock<HttpContext>();
            mockHttpContext.Setup(m => m.User).Returns(principal);

            var controller = new ProfileController(mockUserService.Object, mapper);
            controller.ControllerContext.HttpContext = mockHttpContext.Object;

            var result = controller.Index() as ViewResult;
            var model = result.Model as UserViewModel;

            Assert.Equal(1, model.ID);
        }
        private void ConfigMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<User, UserViewModel>();
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

            var serviceProviderMock = new Mock<IServiceProvider>();

            var urlHelperFactory = new Mock<IUrlHelperFactory>();
            var urlHelper = new Mock<IUrlHelper>();
            var dataDictionaryFactory = new Mock<ITempDataDictionaryFactory>();
            urlHelper.Setup(m => m.IsLocalUrl(It.IsAny<string>())).Returns(true);

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
