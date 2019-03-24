using Classifieds.Domain;
using Classifieds.Repo;
using Classifieds.Repo.Impl;
using Classifieds.Service;
using Classifieds.Service.Impl;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Data.Entity;

namespace Classifieds.Test.Service
{
    [TestClass]
    public class TestService
    {
        private Mock<DbSet<Menu>> mockSet;
        private Mock<ApplicationContext> mockContext;
        private Mock<IMenuRepo> mockMenuRepo;

        public TestService()
        {
            mockSet = new Mock<DbSet<Menu>>();
            mockContext = new Mock<ApplicationContext>();
            mockMenuRepo = new Mock<IMenuRepo>();
            mockSet = new Mock<DbSet<Menu>>();
        }
        [TestMethod]
        public void testService_Create()
        {
            mockContext.Setup(m => m.Set<Menu>()).Returns(mockSet.Object);

            Menu mockMenu = new Menu { ID = 1, Name = "menu 1" };
            //mockMenuRepo.Setup(m => m.find(It.IsAny<System.Int64>())).Returns(mockMenu);


            var menuService = new MenuService(mockMenuRepo.Object);
            menuService.create(mockMenu);
            menuService.save();

            mockMenuRepo.Verify(m => m.create(It.IsAny<Menu>()), Times.Once());
            mockMenuRepo.Verify(m => m.save(), Times.Once());


        }
        [TestMethod]
        public void testService_Delete()
        {
            mockContext.Setup(m => m.Set<Menu>()).Returns(mockSet.Object);

            var menuService = new MenuService(mockMenuRepo.Object);
            menuService.delete(2);
            menuService.save();

            mockMenuRepo.Verify(m => m.delete(It.IsAny<long>()), Times.Once());
            mockMenuRepo.Verify(m => m.save(), Times.Once());
        }

        [TestMethod]
        public void testService_Find()
        {
            
            mockContext.Setup(m => m.Set<Menu>()).Returns(mockSet.Object);

            Menu mockMenu = new Menu { ID = 1, Name = "menu 1" };
            mockMenuRepo.Setup(m => m.find(It.IsAny<System.Int64>())).Returns(mockMenu);


            var menuService = new MenuService(mockMenuRepo.Object);
            var menu = menuService.find(2);

            Assert.AreEqual(1, menu.ID);
            Assert.AreEqual("menu 1", menu.Name);
        }
        [TestMethod]
        public void testService_FindAll()
        {
            mockContext.Setup(m => m.Set<Menu>()).Returns(mockSet.Object);

            List<Menu> mockMenuList = new List<Menu>
            {
                new Menu{ID = 1, Name = "menu 1" },
                new Menu{ID = 2, Name = "menu 2" }
            };
            mockMenuRepo.Setup(m => m.findAll()).Returns(mockMenuList);


            var menuService = new MenuService(mockMenuRepo.Object);
            List<Menu> list = menuService.findAll() as List<Menu>;

            Assert.AreEqual(2, list.Count);
        }
        [TestMethod]
        public void testService_Update()
        {

            mockContext.Setup(m => m.Set<Menu>()).Returns(mockSet.Object);

            var menuService = new MenuService(mockMenuRepo.Object);
            menuService.update(new Menu { ID = 1, Name = "menu 1" });
            menuService.save();

            mockMenuRepo.Verify(m => m.update(It.IsAny<Menu>()), Times.Once());
            mockMenuRepo.Verify(m => m.save(), Times.Once());
        }
    }
}
