using System;
using System.Collections.Generic;
using Xunit;
using Moq;
using Classifieds.Domain.Model;
using Classifieds.Service;
using Classifieds.Controllers;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Classifieds.Web.Models;

namespace Classifieds.XUnitTest
{
    public class TestHomeController
    {
        private IMapper mapper;
        private Mock<IMenuService> mockService;

        public TestHomeController()
        {
            Initialize();
            mockService = new Mock<IMenuService>();
        }
        [Fact]
        public void TestIndex()
        {
            
            IEnumerable<Menu> menus = new List<Menu>
            {
                new Menu{ID=1,Name="menu 1"},
                new Menu{ID=2,Name="menu 2" }
            };

            mockService.Setup(m => m.findByType(It.IsAny<String[]>())).Returns(menus);
            var controller = new HomeController(mockService.Object, mapper);

            var result = controller.Index() as ViewResult;
            List<MenuViewModel> list = result.Model as List<MenuViewModel>;

            Assert.Equal(2, list.Count);
        }

        private void Initialize()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<MenuViewModel, Menu>();
            });

            mapper = config.CreateMapper();
        }
    }
}
