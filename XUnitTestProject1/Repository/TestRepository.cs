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
            menuRepo.create(new Menu { ID = 5, Name = "menu 5" });
            mockContext.SaveChanges();

            var menu = menuRepo.find(5L);

            Assert.Equal(5, mockContext.Menus.Count());
            Assert.Equal("menu 5", mockContext.Menus.Find(5L).Name);

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

            Assert.Equal(3, mockContext.Menus.Count());
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

            Assert.Equal(4, menus.Count());
            Assert.Equal("vehicles", menus.ElementAt(0).Name);
            Assert.Equal("gardening", menus.ElementAt(1).Name);
            Assert.Equal("travel", menus.ElementAt(2).Name);
            Assert.Equal("fashion", menus.ElementAt(3).Name);
        }
       
        private void initContext()
        {
            

            var builder = new DbContextOptionsBuilder<ApplicationContext>()
                .UseInMemoryDatabase<ApplicationContext>("TestDB.mdf");

            var context = new ApplicationContext(builder.Options);

            var menus = new List<Menu>
            {
                new Menu{ID=1, Name="vehicles"},
                new Menu{ID=2, Name="gardening"},
                new Menu{ID=3, Name="travel"},
                new Menu{ID=4, Name="fashion"}
            };

            context.AddRange(menus);
            int changed = context.SaveChanges();
            mockContext = context;
        }

        public void Dispose()
        {
            
        }
    }
    
}
