using Classifieds.Domain.Model;
using Classifieds.Repository;
using Classifieds.Repository.Impl;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Xunit;

namespace Classifieds.XUnitTest.Repository
{
    /// <summary>
    /// Test CRUD operations for GenericRepository using Menu entity
    /// </summary>
    public class TestRepository : IDisposable
    {
        private DbSet<Menu> dbSet;
        private ApplicationContext appContext;

        public TestRepository()
        {
            InitContext();
            dbSet = appContext.Set<Menu>();
        }
        /// <summary>
        /// Test Method { public void Create(T entity) }
        /// </summary>
        [Fact]
        public void Repository_Create()
        {
            var menuRepo = new MenuRepo(appContext);
            menuRepo.Create(new Menu { ID = 6, Name = "menu 6" });
            appContext.SaveChanges();

            Assert.Equal(6, appContext.Menus.Count());
            Assert.Equal("menu 6", appContext.Menus.Find(6L).Name);

        }
        /// <summary>
        /// Test Method { public void Update(T entity) }
        /// </summary>
        [Fact]
        public void Repository_Update()
        {
            var menuRepo = new MenuRepo(appContext);

            Menu menu = menuRepo.Find(2);
            menu.Name = "gardening updated";
            menuRepo.Update(menu);
            appContext.SaveChanges();

            Assert.Equal("gardening updated", appContext.Menus.Find(2L).Name);
        }
        /// <summary>
        /// Test Method { public void Delete(long id) }
        /// </summary>
        [Fact]
        public void Repository_Delete()
        {

            var menuRepo = new MenuRepo(appContext);
            menuRepo.Delete(2);
            menuRepo.Save();

            Assert.Equal(4, appContext.Menus.Count());
        }
        /// <summary>
        /// Test Method { public T Find(long id) }
        /// </summary>
        [Fact]
        public void Repository_Find()
        {
            var menuRepo = new MenuRepo(appContext);
            var menu = menuRepo.Find(5);

            Assert.Equal(5, menu.ID);
            Assert.Equal("cars", menu.Name);
        }
        /// <summary>
        /// Test Method { T Find(long id, Expression<Func<T, Object>>[] includes) }
        /// </summary>
        [Fact]
        public void Repository_FindInclude()
        {
            var menuRepo = new MenuRepo(appContext);

            Expression<Func<Menu, Object>>[] includes =
                {
                    menu => menu.Parent
                };

            Menu result = menuRepo.Find(5, includes);

            Assert.Equal(5, result.ID);
            Assert.Equal("cars", result.Name);
            Assert.Equal(1, result.ParentID);
            Assert.Equal("vehicles", result.Parent.Name);

        }
        /// <summary>
        /// public IEnumerable<T> FindAll()
        /// </summary>
        [Fact]
        public void Repository_FindAll()
        {
            var repo = new MenuRepo(appContext);
            var menus = repo.FindAll();

            Assert.Equal(5, menus.Count());
            //Assert.Equal("vehicles", menus.ElementAt(0).Name);
            //Assert.Equal("gardening", menus.ElementAt(1).Name);
            //Assert.Equal("travel", menus.ElementAt(2).Name);
            //Assert.Equal("fashion", menus.ElementAt(3).Name);
        }
        /// <summary>
        /// Test Method {public IEnumerable<T> FindAll(long id, 
        /// Expression<Func<T, bool>> wherePredicate,
        /// Expression<Func<T, Object>>[] includes)}
        /// </summary>
        [Fact]
        public void Repository_FindAllIncludeWhere()
        {
            var menuRepo = new MenuRepo(appContext);

            Expression<Func<Menu, Object>>[] includes =
            {
                m => m.SubMenus,
                m => m.Parent
            };
            Expression<Func<Menu, bool>> where = m => m.Type == "HOME";

            var results = menuRepo.FindAll(where, includes);

            Assert.Equal(2, results.Count());
        }
        /// <summary>
        ///Create a context and initialize the database with test Data
        ///This method runs before each test.
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
                new Menu{ID=5, Name="cars",Type="SUBMENU",ParentID=1}
            };

            appContext.AddRange(menus);
            int changed = appContext.SaveChanges();
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
