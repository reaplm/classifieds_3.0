using Classifieds.Domain.Model;
using Classifieds.Repository;
using Classifieds.Repository.Impl;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using Xunit;

namespace Classifieds.XUnitTest.Repository
{
    public class TestCategoryRepo : IDisposable
    {
        private ApplicationContext appContext;

        public TestCategoryRepo()
        {
            InitContext();
        }
        /// <summary>
        /// public IEnumerable<T> FindAll(Expression<Func<T, bool>> wherePredicate,
        ///    Expression<Func<T, Object>>[] includes)
        /// </summary>
        [Fact]
        public void Repository_FindAllIncludeWhere()
        {
            var repo = new CategoryRepo(appContext);

            Expression<Func<Category, Object>>[] include =
            {
                c => c.SubCategories
            };
            Expression<Func<Category, bool>> where = c => c.ID == 1;

            List<Category> categories = repo.FindAll(where, include) as List<Category>;
            Category cat = categories.Find(c => c.ID == 1);
            List<Category> subs = categories.Find(c => c.ID == 1).SubCategories as List<Category>;

            Assert.Single(categories);
            Assert.Equal(2, subs.Count);
    
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

            var categories = new List<Category>
            {
                new Category{ID=1, Name="vehicles"},
                new Category{ID=2, Name="gardening"},
                new Category{ID=3, Name="travel"},
                new Category{ID=4, Name="fashion"},
                new Category{ID=5, Name="cars",ParentID=1},
                new Category{ID=6, Name="trucks",ParentID=1}
            };

            appContext.AddRange(categories);
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
