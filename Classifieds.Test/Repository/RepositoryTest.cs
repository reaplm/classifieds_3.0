using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Classifieds.Test.Repository
{
    

    [TestClass]
    public class RepositoryTest
    {
        private Mock<DbSet<Menu>> mockSet;
        private Mock<ApplicationContext> mockContext;

        public RepositoryTest()
        {
            mockSet = new Mock<DbSet<Menu>>();
            mockContext = new Mock<ApplicationContext>();
        }

        [TestMethod]
        public void testRepository_Create()
        {
            List<Menu> data = getData();

            mockSet.As<IQueryable<Menu>>().Setup(m => m.Provider).Returns(data.AsQueryable().Provider);
            mockSet.As<IQueryable<Menu>>().Setup(m => m.ElementType).Returns(data.AsQueryable().ElementType);
            mockSet.As<IQueryable<Menu>>().Setup(m => m.Expression).Returns(data.AsQueryable().Expression);
            mockSet.As<IQueryable<Menu>>().Setup(m => m.GetEnumerator()).Returns(data.AsQueryable().GetEnumerator());

            mockContext.Setup(m => m.Set<Menu>()).Returns(mockSet.Object);
            mockSet.Setup(m => m.Add(It.IsAny<Menu>()))
                .Callback<Menu>(entity => data.Add(entity));

            mockSet.Setup(m => m.Find(It.IsAny<object[]>()))
               .Returns<object[]>(id => data.FirstOrDefault(d => d.ID == (Int64)id[0]));

            var menuRepo = new MenuRepo(mockContext.Object);
            menuRepo.create(new Menu { ID = 5, Name = "menu 5" });
            mockContext.Object.SaveChanges();


            mockSet.Verify(m => m.Add(It.IsAny<Menu>()), Times.Once());
            mockContext.Verify(m => m.SaveChanges(), Times.Once());
            Assert.AreEqual(5, data.Count());

        }
        [TestMethod]
        public void testRepository_Update()
        {
            List<Menu> data = getData();

            mockSet.As<IQueryable<Menu>>().Setup(m => m.Provider).Returns(data.AsQueryable().Provider);
            mockSet.As<IQueryable<Menu>>().Setup(m => m.ElementType).Returns(data.AsQueryable().ElementType);
            mockSet.As<IQueryable<Menu>>().Setup(m => m.Expression).Returns(data.AsQueryable().Expression);
            mockSet.As<IQueryable<Menu>>().Setup(m => m.GetEnumerator()).Returns(data.AsQueryable().GetEnumerator());

            mockContext.Setup(m => m.Set<Menu>()).Returns(mockSet.Object);

            mockSet.Setup(m => m.Find(It.IsAny<object[]>()))
               .Returns<object[]>(id => data.FirstOrDefault(d => d.ID == (Int64)id[0]));

            var menuRepo = new MenuRepo(mockContext.Object);
            
            Menu menu = menuRepo.find(2);
            menu.Name = "menu 2 updated";
            menuRepo.update(menu);
            mockContext.Object.SaveChanges();


           // mockSet.Verify(m => m.(It.IsAny<Menu>()), Times.Once());
            mockContext.Verify(m => m.SaveChanges(), Times.Once());
            Assert.AreEqual("menu 2 updated", data.ElementAt(1).Name);

        }
        [TestMethod]
        public void testRepository_Delete()
        {
            List<Menu> data = getData();

            mockSet.As<IQueryable<Menu>>().Setup(m => m.Provider).Returns(data.AsQueryable().Provider);
            mockSet.As<IQueryable<Menu>>().Setup(m => m.ElementType).Returns(data.AsQueryable().ElementType);
            mockSet.As<IQueryable<Menu>>().Setup(m => m.Expression).Returns(data.AsQueryable().Expression);
            mockSet.As<IQueryable<Menu>>().Setup(m => m.GetEnumerator()).Returns(data.AsQueryable().GetEnumerator());

            mockContext.Setup(m => m.Set<Menu>()).Returns(mockSet.Object);
            mockSet.Setup(m => m.Remove(It.IsAny<Menu>())).Callback<Menu>(entity => data.Remove(entity));
            mockSet.Setup(m => m.Find(It.IsAny<object[]>()))
               .Returns<object[]>(id => data.FirstOrDefault(d => d.ID == (Int64)id[0]));

            var menuRepo = new MenuRepo(mockContext.Object);
            menuRepo.delete(2);

            Assert.AreEqual(3, data.Count());
        }

        [TestMethod]
        public void testRepository_Find()
        {
            IQueryable<Menu> data = getData().AsQueryable();

            
            mockContext.Setup(m => m.Set<Menu>()).Returns(mockSet.Object);

            mockSet.Setup(m => m.Find(It.IsAny<Object[]>()))
                .Returns<Object[]>(id => data.FirstOrDefault(d => d.ID == (Int64)id[0]));

            var menuRepo = new MenuRepo(mockContext.Object);
            var menu = menuRepo.find(2);

            Assert.AreEqual(2, menu.ID);
            Assert.AreEqual("menu 2", menu.Name);


        }
        [TestMethod]    
        public void testRepository_FindAll()
        {
            IQueryable<Menu> data = getData().AsQueryable();

            mockSet.As<IQueryable<Menu>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<Menu>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<Menu>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<Menu>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            //mockContext.Setup(m => m.Menus).Returns(mockSet.Object);
            mockContext.Setup(m => m.Set<Menu>()).Returns(mockSet.Object);

            var repo = new MenuRepo(mockContext.Object);
            var menus = repo.findAll();

            Assert.AreEqual(4, menus.Count());
            Assert.AreEqual("menu 1", menus.ElementAt(0).Name);
            Assert.AreEqual("menu 2", menus.ElementAt(1).Name);
            Assert.AreEqual("menu 3", menus.ElementAt(2).Name);
            Assert.AreEqual("menu 4", menus.ElementAt(3).Name);
        }
        private List<Menu> getData()
        {
            List<Menu> menus = new List<Menu>
            {
                new Menu{ID=1, Name="menu 1"},
                new Menu{ID=2, Name="menu 2"},
                new Menu{ID=3, Name="menu 3"},
                new Menu{ID=4, Name="menu 4"}
            };

            return menus;
        }
    }
}
