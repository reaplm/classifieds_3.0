using System;
using System.Collections.Generic;
using Xunit;
using Moq;
using Classifieds.Domain.Model;
using Classifieds.Service;
using Classifieds.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace Classifieds.XUnitTest
{
    public class TestHomeController
    {
        [Fact]
        public void testIndex()
        {
            var mockService = new Mock<IMenuService>();
            IEnumerable<Menu> menus = new List<Menu>
            {
                new Menu{ID=1,Name="menu 1"},
                new Menu{ID=2,Name="menu 2" }
            };

            mockService.Setup(m => m.findAll()).Returns(menus);
            var controller = new HomeController(mockService.Object);

            var result = controller.Index() as ViewResult;
            List<Menu> list = result.ViewData["Menus"] as List<Menu>;

            Assert.Equal(2, list.Count);
        }
    }
}
