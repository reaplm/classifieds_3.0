using Classifieds.Domain.Enumerated;
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
        /// Test Method { public void Create(T entity) }
        /// </summary>
        [Fact]
        public void Repository_CreateWithChildren()
        {
            List<Menu> subMenus = new List<Menu>
            {
                new Menu{ ID = 8, Name = "menu 8" },
                new Menu{ ID = 9, Name = "menu 9" }
            };
            
            var menu = new Menu {ID = 7, Name = "menu 7", SubMenus = subMenus };

            var menuRepo = new MenuRepo(appContext);
            menuRepo.Create(menu);
            appContext.SaveChanges();

            var menu7 = appContext.Menus.Find(7L);

            Assert.Equal(8, appContext.Menus.Count());
            Assert.Equal(2, menu7.SubMenus.Count());
            Assert.Equal("menu 7", appContext.Menus.Find(7L).Name);
            Assert.Equal("menu 8", appContext.Menus.Find(8L).Name);
            Assert.Equal("menu 9", appContext.Menus.Find(9L).Name);

        }
        /// <summary>
        /// Test Method { public void Create(T entity) }
        /// </summary>
        [Fact]
        public void Repository_CreateEntity()
        {
            List<Menu> subMenus = new List<Menu>
            {
                new Menu{ ID = 8, Name = "menu 8" },
                new Menu{ ID = 9, Name = "menu 9" }
            };

            var menu = new Menu { ID = 7, Name = "menu 7", SubMenus = subMenus };

            var menuRepo = new MenuRepo(appContext);
            var result = menuRepo.CreateEntity(menu);
            appContext.SaveChanges();

            Assert.Equal(8, appContext.Menus.Count());
            Assert.Equal("menu 7", result.Name);
            Assert.Equal(2, result.SubMenus.Count());

        }
        /// <summary>
        /// Test Method { public void Update(T entity) }
        /// </summary>
        [Fact]
        public void Repository_Update()
        {
            var menuRepo = new MenuRepo(appContext);

            Menu menu = menuRepo.Find(2);
            menu.Name = "manage updated";
            menuRepo.Update(menu);
            appContext.SaveChanges();

            Assert.Equal("manage updated", appContext.Menus.Find(2L).Name);
        }
        /// <summary>
        /// Test Method { public void Update(T entity) }
        /// </summary>
        [Fact]
        public void Repository_UpdateWithChildren()
        {
            var advertRepo = new AdvertRepo(appContext);
            var categoryRepo = new CategoryRepo(appContext);
            Category category = categoryRepo.Find(5);
            Advert advert = advertRepo.Find(8);

            advert.Detail.Email = "updateEmail@email.com";
            advert.Detail.Body = "4x4 for sale";
            advert.SubmittedDate = new DateTime(2019, 6, 10);
            advert.Category = category;

            advertRepo.Update(advert);
            appContext.SaveChanges();

            Assert.Equal(new DateTime(2019, 6, 10), appContext.Adverts.Find(8L).SubmittedDate);
            Assert.Equal("4x4 for sale", appContext.Adverts.Find(8L).Detail.Body);
            Assert.Equal("updateEmail@email.com", appContext.Adverts.Find(8L).Detail.Email);
            Assert.Equal(5, appContext.Adverts.Find(8L).Category.ID);
            Assert.Equal(2, appContext.Adverts.Find(8L).Category.ParentID);
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
        /// Test Method { public void Delete(long id) }
        /// </summary>
        [Fact]
        public void Repository_DeleteCascade()
        {

            var advertRepo = new AdvertRepo(appContext);

            Assert.Equal(2, appContext.Adverts.Count());
            Assert.Equal(2, appContext.AdvertDetails.Count());
            Assert.Equal(4, appContext.AdPictures.Count());

            advertRepo.Delete(6);
            advertRepo.Save();

            Assert.Equal(1, appContext.Adverts.Count());
            Assert.Equal(1, appContext.AdvertDetails.Count());
            Assert.Equal(2, appContext.AdPictures.Count());
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
            Assert.Equal("profile", menu.Name);
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
            Assert.Equal("profile", result.Name);
            Assert.Equal(4, result.ParentID);
            Assert.Equal("account", result.Parent.Name);

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
            Expression<Func<Menu, bool>> where = m => m.Type == "SIDEBAR";

            var results = menuRepo.FindAll(where, includes);

            Assert.Equal(3, results.Count());
        }
        /// <summary>
        /// Test cascade update
        /// Test Method public void Update(T entity, 
        /// Object[] keyValues, string[] includes)}
        /// </summary>
        [Fact]
        public void Repository_Update2()
        {
            var advertRepo = new AdvertRepo(appContext);

            AdvertDetail newAdDetail = new AdvertDetail
            {
                ID = 8,
                Title = "Black Toyota for sale in mogoditshane",
                Body = "Black 4x4 Toyota cruiser",
                Email = "pearl@email.com",
                Location = "Mogoditshane",
                AdPictures = new List<AdPicture>
                {
                    new AdPicture
                    {
                        ID = 3,
                        Uuid = "new image",
                        CdnUrl = "new cdn url",
                        Name = "about me sampl.PNG",
                        Size = 135083
                    }
                }
            };

            Advert newAd = new Advert
            {
                ID = 8,
                Status = EnumTypes.AdvertStatus.SUBMITTED.ToString(),
                CategoryID = 6,
                Detail = newAdDetail
            };

            string[] includes = new string[]
            {
                "Detail"
            };

            int changedRows = advertRepo.Update(newAd, keyValues: new object[] { newAd.ID }, 
                includes: includes);

            Advert editedAd = advertRepo.Find(8, new Expression<Func<Advert, object>>[] { x => x.Detail });

            //changedRows is 4, detail is null
            Assert.True(changedRows > 0);
            Assert.Equal("Black Toyota for sale in mogoditshane", editedAd.Detail.Title);
            Assert.Equal("pearl@email.com", editedAd.Detail.Email);
            Assert.Equal("Mogoditshane", editedAd.Detail.Location);
            Assert.Equal(new DateTime(2019, 05, 10), editedAd.PublishedDate);
            Assert.Equal("71406569", editedAd.Detail.Phone);
            Assert.Single(editedAd.Detail.AdPictures);
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
                new Menu{ID=1, Name="home",Type="HOME"},
                new Menu{ID=2, Name="manage", Type="SIDEBAR"},
                new Menu{ID=3, Name="settings",Type="SIDEBAR"},
                new Menu{ID=4, Name="account",Type="SIDEBAR"},
                new Menu{ID=5, Name="profile",Type="SUBMENU",ParentID=4}
            };
            var categories = new List<Category>
            {
                new Category{ID=1, Name="vehicles"},
                new Category{ID=2, Name="gardening"},
                new Category{ID=3, Name="travel"},
                new Category{ID=4, Name="fashion"},
                new Category{ID=5, Name="cars", ParentID = 2}
            };

            var adverts = GetAdvert();

            appContext.AddRange(menus);
            appContext.AddRange(categories);
            appContext.AddRange(adverts);
            int changed = appContext.SaveChanges();
        }
        private List<Advert> GetAdvert()
        {

            

            AdPicture picture1 = new AdPicture
            {
                ID = 1,
                Uuid = "0b83b507-8c11-4c0e-96d2-5fd773d525f7",
                CdnUrl = "https://ucarecdn.com/0b83b507-8c11-4c0e-96d2-5fd773d525f7/",
                Name = "about me sample 3.PNG",
                Size = 135083
            };
            AdPicture picture2 = new AdPicture
            {
                ID = 2,
                Uuid = "c1df9f17-61ad-450a-87f9-d846c312dae0",
                CdnUrl = "https://ucarecdn.com/c1df9f17-61ad-450a-87f9-d846c312dae0/",
                Name = "about me sample 4.PNG",
                Size = 146888
            };
            List<AdPicture> pictures = new List<AdPicture> { picture1, picture2 };

            AdvertDetail advertDetail = new AdvertDetail
            {
                ID = 8,
                Title = "Black Toyota for sale",
                Body = "Black 4x4 Toyota cruiser",
                Email = "pearl@email",
                Phone = "71406569",
                GroupCdn = "GroupCdnValue",
                GroupCount = 2,
                GroupSize = 2048,
                GroupUuid = "GroupUuidValue",
                Location = "Gaborone",
                AdPictures = pictures
            };

            Advert advert = new Advert
            {
                ID = 8,
                Status = EnumTypes.AdvertStatus.SUBMITTED.ToString(),
                CategoryID = 6,
                PublishedDate = new DateTime(2019,05,10),
                Detail = advertDetail
            };

            //Second Advert
            AdPicture picture3 = new AdPicture
            {
                ID = 3,
                Uuid = "0b83b507-8c11-4c0e-96d2-5fd773d525f7",
                CdnUrl = "https://ucarecdn.com/0b83b507-8c11-4c0e-96d2-5fd773d525f7/",
                Name = "about me sample 3.PNG",
                Size = 135083
            };
            AdPicture picture4 = new AdPicture
            {
                ID = 4,
                Uuid = "c1df9f17-61ad-450a-87f9-d846c312dae0",
                CdnUrl = "https://ucarecdn.com/c1df9f17-61ad-450a-87f9-d846c312dae0/",
                Name = "about me sample 4.PNG",
                Size = 146888
            };
            List<AdPicture> ad2Pictures = new List<AdPicture> { picture3, picture4 };

            AdvertDetail advertDetail2 = new AdvertDetail
            {
                ID = 6,
                Title = "Black Toyota for sale",
                Body = "Black 4x4 Toyota cruiser",
                Email = "pearl@email",
                Phone = "71406569",
                GroupCdn = "GroupCdnValue",
                GroupCount = 2,
                GroupSize = 2048,
                GroupUuid = "GroupUuidValue",
                Location = "Gaborone",
                AdPictures = ad2Pictures
            };

            Advert advert2 = new Advert
            {
                ID = 6,
                Status = EnumTypes.AdvertStatus.SUBMITTED.ToString(),
                CategoryID = 2,
                PublishedDate = new DateTime(2019, 05, 10),
                Detail = advertDetail2
            };
            List<Advert> adverts = new List<Advert>();
            adverts.Add(advert);
            adverts.Add(advert2);

            return adverts;
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
            appContext.Categories.RemoveRange(appContext.Categories);
            appContext.Adverts.RemoveRange(appContext.Adverts);
            appContext.AdvertDetails.RemoveRange(appContext.AdvertDetails);
            appContext.AdPictures.RemoveRange(appContext.AdPictures);
            int changed = appContext.SaveChanges();
        }
    }
}
