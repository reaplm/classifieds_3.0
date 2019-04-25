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
        private DbSet<Menu> dbSet;
        private ApplicationContext appContext;

        public TestMenuRepo()
        {

            InitContext();
            dbSet = appContext.Set<Menu>();
        }
        /// <summary>
        /// Test { public IEnumerable<Menu> FindByType(String[] types) }
        /// </summary>
        [Fact]
        public void MenuRepo_FindByType()
        {
            var menuRepo = new MenuRepo(appContext);
            var menus = menuRepo.FindByType(new String[] { "HOME" });
            IEnumerable<Menu> subMenu = menus.ElementAt(0).SubMenus;

            Assert.Equal(2, menus.Count());
            Assert.Equal(2, subMenu.Count());

        }
        /// <summary>
        /// Create a context and initialize the database with test Data
        /// This method runs before each test.
        ///
        /// </summary>
        private void InitContext()
        {

            var builder = new DbContextOptionsBuilder<ApplicationContext>()
                .UseInMemoryDatabase<ApplicationContext>("TestDB.mdf");

            appContext = new ApplicationContext(builder.Options);

            var menus = new List<Menu>
            {
                new Menu{ID=1, Name="vehicles",Type="HOME"},
                new Menu{ID=2, Name="gardening", Type="HOME"},
                new Menu{ID=3, Name="travel",Type="SIDEBAR"},
                new Menu{ID=4, Name="fashion",Type="SUBMENU"},
                new Menu{ID=5, Name="cars",Type="SUBMENU",ParentID=1},
                new Menu{ID=6, Name="trucks",Type="SUBMENU",ParentID=1}
            };

            appContext.AddRange(menus);
            int changed = appContext.SaveChanges();
            //appContext = context;
        }
        /// <summary>
        /// Clear the database in preparation for the next test.
        /// Since each test executes the initContext method, it
        /// is necessary to clear the database before the next 
        /// test, otherwise the tests fail during insert stage.
        /// </summary>
        public void Dispose()
        {
            appContext.Menus.RemoveRange(appContext.Menus);
            int changed = appContext.SaveChanges();
        }
    }
}
