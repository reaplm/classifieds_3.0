﻿using AutoMapper;
using Classifieds.Domain.Model;
using Classifieds.Repository.Impl;
using Classifieds.Service;
using Classifieds.Service.Impl;
using Classifieds.Web.Controllers;
using Classifieds.Web.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Classifieds.XUnitTest.Controller
{
    public class TestClassifiedsController
    {
        private IMapper mapper;
        private Mock<IAdvertService> mockAdvertService;
        private Mock<IMenuService> mockMenuService;
        private Mock<ClaimsPrincipal> mockClaimsPrincipal;

        public TestClassifiedsController()
        {
            Inialize();
            mockAdvertService = new Mock<IAdvertService>();
            mockMenuService = new Mock<IMenuService>();
            mockClaimsPrincipal = new Mock<ClaimsPrincipal>();
        }
        /// <summary>
        /// Test for: public IActionResult Index()
        /// </summary>
        [Fact]
        public void TestIndex()
        {
            IEnumerable<Advert> adverts = new List<Advert>
            {
                new Advert{ID=1,Status="SUBMITTED"},
                new Advert{ID=2,Status="APPROVED"},
                new Advert{ID=3,Status="APPROVED"}
            };

            var menuService = new Mock<IMenuService>();
            var advertService = new Mock<IAdvertService>();
            advertService.Setup(m => m.findAll()).Returns(adverts);

            var controller = new ClassifiedsController(advertService.Object, 
                menuService.Object, mapper);
            var result = controller.Index() as ViewResult;
            var list = result.Model as List<Advert>;

            Assert.Equal(3, list.Count());

        }
        /// <summary>
        /// Test for: public IActionResult Category(int id)
        /// //Classifieds/Category/id
        /// </summary>
        [Fact]
        public void TestCategory()
        {
            IEnumerable<Advert> adverts = new List<Advert>
            {
                new Advert{ID=1,Status="SUBMITTED"},
                new Advert{ID=2,Status="APPROVED"},
                new Advert{ID=3,Status="APPROVED"}
            };

            var menuService = new Mock<IMenuService>();
            var advertService = new Mock<IAdvertService>();
            advertService.Setup(m => m.findByCategory(It.IsAny<int>())).Returns(adverts);

            var controller = new ClassifiedsController(advertService.Object, 
                menuService.Object, mapper);
            var result = controller.Category(2) as ViewResult;
            var list = result.Model as List<Advert>;

            Assert.Equal(3, list.Count());

        }
        /// <summary>
        /// Test for: public IActionResult Create()
        /// Mocks authentication, HttpContext
        /// </summary>
        [Fact]
        public void TestCreate_GET()
        {
            IEnumerable<Menu> menus = GetMenuList();

            //Setup to allow query of authenticated user
            //  (HttpContext.User.Claims.FirstOrDefault(u => u.Type == "UserId").Value)
            var mockIdentity = new GenericIdentity("User");
            mockIdentity.AddClaim(new Claim("UserId", "1"));
            var principal = new GenericPrincipal(mockIdentity, null);

            
            var mockHttpContext = new Mock<HttpContext>();
            mockHttpContext.Setup(m => m.User).Returns(principal);
            
            mockMenuService.Setup(m => m.findByType(It.IsAny<String[]>()))
                .Returns(menus);

           var controller = new ClassifiedsController(mockAdvertService.Object, 
                mockMenuService.Object, mapper);

            controller.ControllerContext.HttpContext = mockHttpContext.Object;

            var result = controller.Create() as ViewResult;
            var menuList = result.ViewData["Menus"] as IEnumerable<SelectListItem>;
            var subMenus = result.ViewData["SubMenus"] as IEnumerable<SelectListItem>;


            Assert.IsType< AdvertViewModel>(result.Model);
            Assert.Equal(3, menuList.Count());
            Assert.Empty(subMenus);
        }
        /// <summary>
        /// Test for: public IActionResult Create(AdvertViewModel model)
        /// Test when ModelState.IsValid = false
        /// </summary>
        [Fact]
        public void TestCreateInvalidModelState_POST()
        {
            var menus = GetMenuList();
            var submenus = GetSubMenuList();
            AdvertViewModel model = new AdvertViewModel();
            model.MenuID = 2;
            model.ParentID = 1;

            mockMenuService.Setup(x => x.findByType(It.IsAny<String[]>()))
                .Returns(menus);
            mockMenuService.Setup(x => x.findAll(It.IsAny<long>()))
                .Returns(submenus);

            var controller = new ClassifiedsController(mockAdvertService.Object, 
                mockMenuService.Object, mapper);
            controller.ModelState.AddModelError("menu", "please select menu");//set ModelState to be invalid

            var result = controller.Create(model) as ViewResult;
            var list = result.ViewData["Menus"] as IEnumerable<SelectListItem>;
            var subs = result.ViewData["SubMenus"] as IEnumerable<SelectListItem>;

            Assert.Equal(3, list.Count());
            Assert.Equal(2, subs.Count());
            Assert.True(subs.FirstOrDefault(x => x.Value.Equals("2")).Selected);
            Assert.True(list.FirstOrDefault(x => x.Value.Equals("1")).Selected);
        }
        /// <summary>
        /// Test for: public IActionResult Create(AdvertViewModel model)
        /// Create a new advert and redict on success
        /// </summary>
        [Fact]
        public void TestCreateValidModelState_POST()
        {
            AdvertViewModel model = new AdvertViewModel();

            var controller = new ClassifiedsController(mockAdvertService.Object, mockMenuService.Object, mapper);
            var result = controller.Create(model) as RedirectToActionResult;

            Assert.Equal("Index", result.ActionName);
            Assert.Equal("Classifieds", result.ControllerName);
            mockAdvertService.Verify(m => m.create(It.IsAny<Advert>()), Times.Once());
            mockAdvertService.Verify(m => m.save(), Times.Once());

        }
        /// <summary>
        /// Initialize AutoMapper
        /// </summary>
        private void Inialize()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<AdvertViewModel, Advert>();
                cfg.CreateMap<AdvertDetailViewModel, AdvertDetail>();
            });

            mapper = config.CreateMapper();
        }
        /// <summary>
        /// Data (Menu SelectList)
        /// </summary>
        /// <returns></returns>
        private IEnumerable<Menu> GetMenuList()
        {

            IEnumerable<Menu> menus = new List<Menu>
            {
                new Menu{ID=1,Name="menu1"},
                new Menu{ID=2,Name="menu2"},
                new Menu{ID=3,Name="menu3"},
            };

            return menus;
        }
        /// <summary>
        /// Data (SubMenu SelectList)
        /// </summary>
        /// <returns></returns>
        private IEnumerable<Menu> GetSubMenuList()
        {

            IEnumerable<Menu> menus = new List<Menu>
            {
                new Menu{ID=1,Name="summenu1"},
                new Menu{ID=2,Name="submenu2"}
            };

            return menus;
        }
        /// <summary>
        /// Mock Authentication Service
        /// </summary>
        /// <returns></returns>
        private IServiceProvider ServiceProviderMock()
        {
            var authServiceMock = new Mock<IAuthenticationService>();
            var urlHelperFactory = new Mock<IUrlHelperFactory>();
            var dataDictionaryFactory = new Mock<ITempDataDictionaryFactory>();

            var serviceProviderMock = new Mock<IServiceProvider>();
            serviceProviderMock.Setup(m => m.GetService(typeof(IAuthenticationService)))
                .Returns(authServiceMock.Object);
            serviceProviderMock.Setup(m => m.GetService(typeof(Microsoft.AspNetCore.Mvc.Routing.IUrlHelperFactory)))
                .Returns(urlHelperFactory.Object);
            serviceProviderMock.Setup(m => m.GetService(typeof(ITempDataDictionaryFactory)))
                .Returns(dataDictionaryFactory.Object);

            return serviceProviderMock.Object;
        }
    }
}
