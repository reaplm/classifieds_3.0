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
using System.Collections.Generic;
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
        private Mock<IUserDetailService> mockUserDetailService;
        private Mock<IAddressService> mockAddressService;
        private Mock<ClaimsPrincipal> mockClaimsPrincipal;
        private IMapper mapper;

        public TestProfileController()
        {
            ConfigMapper();
            mockUserService = new Mock<IUserService>();
            mockUserDetailService = new Mock<IUserDetailService>();
            mockAddressService = new Mock<IAddressService>();
            mockClaimsPrincipal = new Mock<ClaimsPrincipal>();
        }
        [Fact]
        public void TestEditAddress_GET()
        {

            Address address = new Address { ID = 1, PostAddress1 = "P O Box2361", State = "gaborone" };
            UserDetail userDetail = new UserDetail { ID = 1, FirstName = "Pearl", Address = address };


            var mockIdentity = new GenericIdentity("User");
            mockIdentity.AddClaim(new Claim("UserId", "1"));
            var principal = new GenericPrincipal(mockIdentity, null);


            var mockHttpContext = new Mock<HttpContext>();
            mockHttpContext.Setup(m => m.User).Returns(principal);

            mockUserDetailService.Setup(m => m.Find(It.IsAny<Expression<Func<UserDetail, bool>>>(),
                It.IsAny<Expression<Func<UserDetail, object>>[]>())).Returns(userDetail);

            var controller = new ProfileController(mockUserService.Object,
                mockAddressService.Object, mockUserDetailService.Object, mapper);
            controller.ControllerContext.HttpContext = mockHttpContext.Object;

            var result = controller.EditAddress() as PartialViewResult;
            UserDetailViewModel model = result.Model as UserDetailViewModel;

            Assert.Equal("Pearl",model.FirstName);
        }
        [Fact]
        public void EditAddressModelStateIsValid_POST()
        {
            AddressViewModel address = new AddressViewModel { ID = 1, PostAddress1 = "P O Box2361", State = "gaborone" };
            UserDetailViewModel userDetail = new UserDetailViewModel { ID = 1, FirstName = "Pearl", Address = address };

            Address addresses1 =  new Address{ ID = 1, PostAddress1 = "P O Box23", State = "gabs"};
            UserDetail userDetail1 = new UserDetail { ID = 1, FirstName = "Pearl", Address = addresses1 };

            // Microsoft.AspNetCore.Mvc.ControllerBase.HttpContext.get returned

            mockUserDetailService.Setup(m => m.Find(It.IsAny<long>(),
               It.IsAny<Expression<Func<UserDetail, object>>[]>()))
               .Returns(userDetail1);

            var controller = new ProfileController(mockUserService.Object,
               mockAddressService.Object, mockUserDetailService.Object, mapper);

            controller.ControllerContext.HttpContext = new DefaultHttpContext();

            var result = controller.EditAddress(userDetail) as JsonResult;

            Assert.Equal(201, controller.HttpContext.Response.StatusCode);
            Assert.Equal("Address Saved!", result.Value);
        }
        [Fact]
        public void EditAddressModelStateIsInvalid_POST()
        {
            GenericIdentity mockIdentity = new GenericIdentity("User");
            mockIdentity.AddClaim(new Claim("UserId", "1"));
            var principal = new GenericPrincipal(mockIdentity, null);

            AddressViewModel address = new AddressViewModel { ID = 1, PostAddress1 = "P O Box2361", State = "gaborone" };
            UserDetailViewModel userDetail = new UserDetailViewModel { ID = 1, FirstName = "Pearl", Address = address };

            var controller = new ProfileController(mockUserService.Object,
               mockAddressService.Object, mockUserDetailService.Object, mapper);

            var mockHttpContext = new Mock<HttpContext>();
            mockHttpContext.Setup(m => m.User).Returns(principal);



            controller.ControllerContext.HttpContext = new DefaultHttpContext();
            controller.ControllerContext.HttpContext.User = principal;

            controller.ModelState.AddModelError("State", "State is required");

            var result = controller.EditAddress(userDetail) as PartialViewResult;
            var model = result.Model as UserDetailViewModel;

            Assert.Equal("Pearl", model.FirstName);
            Assert.Equal("P O Box2361", model.Address.PostAddress1);
            Assert.Equal("gaborone", model.Address.State);
            Assert.Equal(200, controller.HttpContext.Response.StatusCode);
        }
        private void ConfigMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Address,AddressViewModel>();
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
