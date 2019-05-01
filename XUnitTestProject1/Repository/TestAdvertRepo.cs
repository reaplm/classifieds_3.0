using Classifieds.Domain.Enumerated;
using Classifieds.Domain.Model;
using Classifieds.Repository;
using Classifieds.Repository.Impl;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Classifieds.XUnitTest.Repository
{

    public class TestAdvertRepo : IDisposable
    {
        private ApplicationContext appContext;
        private DbSet<Advert> mockSet;

        public TestAdvertRepo()
        {
            InitContext();
            mockSet = appContext.Set<Advert>();
        }
        /// <summary>
        /// 
        /// </summary>
        [Fact]
        public void FindByCategory()
        {
            var repo = new AdvertRepo(appContext);
            IEnumerable<Advert> adverts = repo.FindByCategory(6);

            Assert.Equal(3, adverts.Count());
        }
        /// <summary>
        /// Create an advert that has pictures
        /// </summary>
        [Fact]
        public void Create()
        {
            Advert advert = GetAdvert();

            AdvertRepo repo = new AdvertRepo(appContext);
            repo.Create(advert);
            repo.Save();

            IEnumerable<Advert> adverts = repo.FindAll();
            Advert ad = adverts.FirstOrDefault(x => x.ID == 8);



            Assert.Equal(8, adverts.Count());
            Assert.Equal("Black Toyota for sale", ad.Detail.Title);
            Assert.Equal(2, ad.Detail.AdPictures.Count());

        }
        /// <summary>
        /// Test data
        /// </summary>
        /// <returns></returns>
        private Advert GetAdvert()
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
                Detail = advertDetail
            };

            return advert;
        }
        /// <summary>
        /// Initialize
        /// </summary>
        private void InitContext()
        {
            var builder = new DbContextOptionsBuilder<ApplicationContext>()
                .UseInMemoryDatabase("TestDB");

            var context = new ApplicationContext(builder.Options);

            var menus = new List<Menu>
            {
                new Menu{ID=1, Name="vehicles",Type="HOME"},
                new Menu{ID=2, Name="gardening", Type="HOME"},
                new Menu{ID=3, Name="travel",Type="SIDEBAR"},
                new Menu{ID=4, Name="electronics",Type="SUBMENU"},
                new Menu{ID=5, Name="property",Type="HOME"},
                new Menu{ID=6, Name="cars",Type="SUBMENU",ParentID=1}
            };

            List<Advert> adverts = new List<Advert>
            {
                new Advert{ID=1, Status="SUBMITTED", SubmittedDate= new DateTime(2019,1,3), CategoryID=5},
                new Advert{ID=2, Status="REJECTED", SubmittedDate= new DateTime(2018,10,5), CategoryID=6},
                new Advert{ID=3, Status="APPROVED", SubmittedDate= new DateTime(2018,12,20), CategoryID=6},
                new Advert{ID=4, Status="SUBMITTED", SubmittedDate= new DateTime(2019,3,11), CategoryID=6},
                new Advert{ID=5, Status="APPROVED", SubmittedDate= new DateTime(2018,10,15), CategoryID=4},
                new Advert{ID=6, Status="SUBMITTED", SubmittedDate= new DateTime(2019,2,28), CategoryID=5 },
                new Advert{ID=7, Status="SUBMITTED", SubmittedDate= new DateTime(2018,7,3), CategoryID=4}
            };

            List<AdvertDetail> advertDetails = new List<AdvertDetail>
            {
                new AdvertDetail{ID=1,Title="room for rent", Body="A LARGE ROOM- can be shared by 2 people", Email="my@email.com",AdvertID=1},
                new AdvertDetail{ID=2,Title="2011 BMW120i", Body="2011 bmw120i Manual gear 150000km 80k", Email="my@email.com",AdvertID=2},
                new AdvertDetail{ID=3,Title="Tyres, Mag Wheels", Body="Your Professional Tyre Fitment Centre", Email="my@email.com", AdvertID=3},
                new AdvertDetail{ID=4,Title="GOLF POLO GTI MODEL 2013", Body="Full serviced car.Aircon sound system.Price 130000 negotiable", Email="my@email.com", AdvertID=4},
                new AdvertDetail{ID=5,Title="Handheld Car Vacuum Cleaners", Body="Fine Living Handheld Vacuum Cleaner", Email="my@email.com", AdvertID=5},
                new AdvertDetail{ID=6,Title="3 bedroom bhc house for Rent", Body="3 bedroom bhc house available in Gaborone ", Email="my@email.com", AdvertID=6},
                new AdvertDetail{ID=7,Title="Samsung J1 Ace For Sale",Body="3month Samsung J1 Ace For Sale. P650 ", Email="my@email.com", AdvertID=7}
            };

            context.AddRange(menus);
            context.AddRange(adverts);
            context.AddRange(advertDetails);
            int changed = context.SaveChanges();
            appContext = context;

        }
        /// <summary>
        /// Setup next test
        /// </summary>
        public void Dispose()
        {
            appContext.Menus.RemoveRange(appContext.Menus);
            appContext.Adverts.RemoveRange(appContext.Adverts);
            appContext.AdvertDetails.RemoveRange(appContext.AdvertDetails);
            int changed = appContext.SaveChanges();
        }
    }
}
