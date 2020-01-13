using AutoMapper;
using Classifieds.Domain.Data;
using Classifieds.Domain.Model;
using Classifieds.Service;
using Classifieds.Web.Controllers;
using Classifieds.Web.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
    public class TestAdminController
    {
        private Mock<IMenuService> mockMenuService;
        private Mock<IUserService> mockUserService;
        private Mock<IAdvertService> mockAdvertService;
        private Mock<ICategoryService> mockCategoryService;
        private Mock<ILikeService> mockLikeService;
        private IMapper mapper;

        public TestAdminController()
        {
            mockMenuService = new Mock<IMenuService>();
            mockUserService = new Mock<IUserService>();
            mockAdvertService = new Mock<IAdvertService>();
            mockCategoryService = new Mock<ICategoryService>();
            mockLikeService = new Mock<ILikeService>();
            Initialize();
        }
        [Fact]
        public void Index()
        {
            var mockSession = new Mock<ISession>();
        
            IEnumerable <Menu> menus = FindAll();

            mockMenuService.Setup(m => m.FindAll(It.IsAny<Expression<Func<Menu, bool>>>(),
                It.IsAny<Expression<Func<Menu, object>>[]>()))
                .Returns(menus);

            var controller = new AdminController(mockMenuService.Object, mockUserService.Object,
                mockAdvertService.Object, mockCategoryService.Object, mockLikeService.Object, mapper);

            controller.ControllerContext = new ControllerContext();
            controller.ControllerContext.HttpContext = new DefaultHttpContext();
            controller.ControllerContext.HttpContext.Session = mockSession.Object;

            var result = controller.Index() as ViewResult;
    
        }
       [Fact]
       public void Menus()
        {
            var menus = FindAll();

            mockMenuService.Setup(m => m.FindAll(It.IsAny<Expression<Func<Menu, bool>>>(),
                It.IsAny<Expression<Func<Menu, object>>[]>())).Returns(menus);
            var controller = new AdminController(mockMenuService.Object, mockUserService.Object,
                mockAdvertService.Object, mockCategoryService.Object, mockLikeService.Object, mapper);
            var result = controller.Menus() as ViewResult;
            List<MenuViewModel> model = result.Model as List<MenuViewModel>;

            Assert.Equal(6, model.Count);
        }
        [Fact]
        public void CountAdvertByStatus()
        {
            var data = new List<CountPercentSummary>
            {
                new CountPercentSummary{Column="APPROVED", Count=2,Percent=28.57},
                new CountPercentSummary{Column="REJECTED", Count=1,Percent=14.29},
                new CountPercentSummary{Column="SUBMITTED", Count=4,Percent=57.14}
            };
            mockAdvertService.Setup(m => m.AdvertCountByStatus()).Returns(data);
            var controller = new AdminController(mockMenuService.Object, mockUserService.Object,
                mockAdvertService.Object, mockCategoryService.Object, mockLikeService.Object, mapper);

            var result = controller.CountAdvertByStatus() as List<CountPercentSummary>;

            Assert.Equal(3, result.Count);
        }
        /// <summary>
        /// Test for { public IActionResult Favourites() }
        /// </summary>
        [Fact]
        public void Favourites()
        {
            var mockIdentity = new GenericIdentity("User");
            mockIdentity.AddClaim(new Claim("UserId", "5"));
            var principal = new GenericPrincipal(mockIdentity, null);

            var mockHttpContext = new Mock<HttpContext>();
            mockHttpContext.Setup(m => m.User).Returns(principal);

            var likes = new List<Like>
            {
                new Like{ID = 1, UserID = 5, AdvertID = 6},
                new Like{ID = 1, UserID = 5, AdvertID = 2},
                new Like{ID = 1, UserID = 5, AdvertID = 10}
            };
            mockLikeService.Setup(m => m.FindByUser(It.IsAny<long>())).Returns(likes);

            var controller = new AdminController(mockMenuService.Object, mockUserService.Object,
                mockAdvertService.Object, mockCategoryService.Object, mockLikeService.Object, mapper);
            controller.ControllerContext.HttpContext = mockHttpContext.Object; //you need access to claims object

            var result = controller.Favourites() as ViewResult;
            List<LikeViewModel> model = result.Model as List<LikeViewModel>;

            Assert.Equal(3, model.Count);
        }
        private IEnumerable<Menu> FindAll()
        {
            var menus = new List<Menu>
            {
                new Menu{ID=1, Name="vehicles",Type="HOME"},
                new Menu{ID=2, Name="gardening", Type="HOME"},
                new Menu{ID=3, Name="travel",Type="SIDEBAR"},
                new Menu{ID=4, Name="fashion",Type="SUBMENU"},
                new Menu{ID=5, Name="cars",Type="SUBMENU",ParentID=1},
                new Menu{ID=6, Name="trucks",Type="SUBMENU",ParentID=1}
            };

            return menus;
        }
        private void Initialize()
        {
            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Menu, MenuViewModel>();
            });

            mapper = mapperConfig.CreateMapper();

        }
        private IServiceProvider ServiceProviderMock()
        {
            var iSessionMock = new Mock<ISession>();
           

            var serviceProviderMock = new Mock<IServiceProvider>();
            serviceProviderMock.Setup(m => m.GetService(typeof(ISession)))
                .Returns(iSessionMock.Object);
           

            return serviceProviderMock.Object;
        }
    }
}
