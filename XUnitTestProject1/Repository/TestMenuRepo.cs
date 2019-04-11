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
    public class TestMenuRepo : IDisposable
    {
        private DbSet<Menu> mockSet;
        private ApplicationContext mockContext;

        public TestMenuRepo()
        {
           
            initContext();
            mockSet = mockContext.Set<Menu>();
        }
       [Fact]
        public void testFindByType()
        {
            var menuRepo = new MenuRepo(mockContext);
            var menus = menuRepo.findByType(new String[] { "HOME" });
            IEnumerable<Menu> subMenu = menus.ElementAt(0).SubMenus;

            Assert.Equal(2, menus.Count());
            Assert.Equal(2, subMenu.Count());

        }
        [Fact]
        public void TestFindAllInt()
        {
            var menuRepo = new MenuRepo(mockContext);
            var menus = menuRepo.findAll(1);

            Assert.Equal(2, menus.Count());
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
                new Menu{ID=5, Name="cars",Type="SUBMENU",ParentID=1},
                new Menu{ID=6, Name="trucks",Type="SUBMENU",ParentID=1}
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
