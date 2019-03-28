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

    public class TestAdvertRepo
    {
        private ApplicationContext mockContext;
        private DbSet<Advert> mockSet;
     
        public TestAdvertRepo()
        {
            initContext();
            mockSet = mockContext.Set<Advert>();
        }
        [Fact]
        public void testFindByCategory()
        {
            var repo = new AdvertRepo(mockContext);
            IEnumerable<Advert> adverts = repo.findByCategory(6);

            IEnumerable<long> advertID = new List<long> { 2, 3, 4 };

            Assert.Equal(3, adverts.Count());
            //Assert.True(adverts.All(advertID.Contains((x => x.ID)));
        }

        private void initContext()
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
                new Advert{ID=1, Status="SUBMITTED", SubmittedDate= new DateTime(2019,1,3), MenuID=5},
                new Advert{ID=2, Status="REJECTED", SubmittedDate= new DateTime(2018,10,5), MenuID=6},
                new Advert{ID=3, Status="APPROVED", SubmittedDate= new DateTime(2018,12,20), MenuID=6},
                new Advert{ID=4, Status="SUBMITTED", SubmittedDate= new DateTime(2019,3,11), MenuID=6},
                new Advert{ID=5, Status="APPROVED", SubmittedDate= new DateTime(2018,10,15), MenuID=4},
                new Advert{ID=6, Status="SUBMITTED", SubmittedDate= new DateTime(2019,2,28), MenuID=5 },
                new Advert{ID=7, Status="SUBMITTED", SubmittedDate= new DateTime(2018,7,3), MenuID=4}
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
            mockContext = context;

        }
    }
}
