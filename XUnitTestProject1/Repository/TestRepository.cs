using Classifieds.Domain.Model;
using Classifieds.Repository;
using Classifieds.Repository.Impl;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Classifieds.XUnitTest.Repository
{
    /// <summary>
    /// Test class for Classifieds.Repository.GenericRepo using Menu objects
    /// </summary>
    public class TestRepository : IDisposable
    {
        private DbSet<Menu> mockSet;
        private ApplicationContext mockContext;

        public TestRepository()
        {
            initContext();
            mockSet = mockContext.Set<Menu>();
        }
        [Fact]
        public void testRepository_Create()
        {
            var menuRepo = new MenuRepo(mockContext);
            menuRepo.create(new Menu { ID = 6, Name = "menu 6" });
            mockContext.SaveChanges();

            var menu = menuRepo.find(5L);

            Assert.Equal(6, mockContext.Menus.Count());
            Assert.Equal("menu 6", mockContext.Menus.Find(6L).Name);

        }
        [Fact]
        public void testRepository_Update()
        {
            var menuRepo = new MenuRepo(mockContext);

            Menu menu = menuRepo.find(2);
            menu.Name = "gardening updated";
            menuRepo.update(menu);
            mockContext.SaveChanges();

            
            Assert.Equal("gardening updated", mockContext.Menus.Find(2L).Name);

        }
        [Fact]
        public void testRepository_Delete()
        {

            var menuRepo = new MenuRepo(mockContext);
            menuRepo.delete(2);
            menuRepo.save();

            Assert.Equal(4, mockContext.Menus.Count());
        }

        [Fact]
        public void testRepository_Find()
        {
            var menuRepo = new MenuRepo(mockContext);
            var menu = menuRepo.find(2L);

            Assert.Equal(2, menu.ID);
            Assert.Equal("gardening", menu.Name);


        }
        [Fact]
        public void testRepository_FindAll()
        {
            var repo = new MenuRepo(mockContext);
            var menus = repo.findAll();

            Assert.Equal(5, menus.Count());
            Assert.Equal("vehicles", menus.ElementAt(0).Name);
            Assert.Equal("gardening", menus.ElementAt(1).Name);
            Assert.Equal("travel", menus.ElementAt(2).Name);
            Assert.Equal("fashion", menus.ElementAt(3).Name);
        }
       [Fact]
        public void testFindByType()
        {
            var menuRepo = new MenuRepo(mockContext);
            var menus = menuRepo.findByType(new String[] { "HOME" });
            IEnumerable<Menu> subMenu = menus.ElementAt(0).SubMenus;

            Assert.Equal(2, menus.Count());
            Assert.Single(subMenu);

        }
        /**
         * Create a context and initialize the database with test Data
         * This method runs before each test.
         * 
         * */
        private void initContext()
        {
            

            var builder = new DbContextOptionsBuilder<ApplicationContext>()
                .UseInMemoryDatabase<ApplicationContext>("TestDB.mdf");

            var context = new ApplicationContext(builder.Options);

            var menus = new List<Menu>
            {
                new Menu{ID=1, Name="vehicles",Type="HOME"},
                new Menu{ID=2, Name="gardening", Type="HOME"},
                new Menu{ID=3, Name="travel",Type="SIDEBAR"},
                new Menu{ID=4, Name="fashion",Type="SUBMENU"},
                new Menu{ID=5, Name="cars",Type="SUBMENU",ParentID=1}
            };

            context.AddRange(menus);
            int changed = context.SaveChanges();
            mockContext = context;
        }
        /**
         * Clear the database in preparation for the next test.
         * Since each test executes the initContext method, it
         * is necessary to clear the database before the next 
         * test, otherwise the tests fail during insert stage.
         **/
        public void Dispose()
        {
            mockContext.Menus.RemoveRange(mockContext.Menus);
            int changed = mockContext.SaveChanges();
        }
    }
    
}
