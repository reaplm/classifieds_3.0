using Classifieds.Domain.Model;
using Classifieds.Repository;
using Classifieds.Service;
using Classifieds.Web.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Moq;
using MySqlX.XDevAPI.Common;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Classifieds.XUnitTest.Controller
{
    public class TestMenuController
    {
        private Mock<IMenuService> mockMenuService;

        public TestMenuController()
        {
            mockMenuService = new Mock<IMenuService>();
        }
        [Fact]
        public void TestSubMenus()
        {
            var menus = FindAll();

            mockMenuService.Setup(m => m.findAll(It.IsAny<long>()))
                .Returns(menus);

            var controller = new MenuController(mockMenuService.Object);
            OkObjectResult result = controller.SubMenus(1) as OkObjectResult;
            List<Menu> model = Assert.IsType<List<Menu>>(result.Value);

            //var model = result.Value;

            Assert.Equal(200, result.StatusCode);
            //Assert.Equal(6, model.);

        }
      
        private List<Menu> FindAll()
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
    }
}
