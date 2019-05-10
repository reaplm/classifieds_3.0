using AutoMapper;
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
using System.Text;
using Xunit;

namespace Classifieds.XUnitTest.Controller
{
    public class TestAdminController
    {
        private Mock<IMenuService> mockMenuService;
        private IMapper mapper;

        public TestAdminController()
        {
            mockMenuService = new Mock<IMenuService>();
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

            var controller = new AdminController(mockMenuService.Object, mapper);

            controller.ControllerContext = new ControllerContext();
            controller.ControllerContext.HttpContext = new DefaultHttpContext();
            controller.ControllerContext.HttpContext.Session = mockSession.Object;

            var result = controller.Index() as ViewResult;
    
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
