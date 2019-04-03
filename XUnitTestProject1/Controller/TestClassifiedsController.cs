using Classifieds.Domain.Model;
using Classifieds.Repository.Impl;
using Classifieds.Service;
using Classifieds.Service.Impl;
using Classifieds.Web.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Classifieds.XUnitTest.Controller
{
    public class TestClassifiedsController
    {

        public TestClassifiedsController()
        {
            
        }
        [Fact]
        public void TestIndex()
        {
            IEnumerable<Advert> adverts = new List<Advert>
            {
                new Advert{ID=1,Status="SUBMITTED"},
                new Advert{ID=2,Status="APPROVED"},
                new Advert{ID=3,Status="APPROVED"}
            };

            var advertService = new Mock<IAdvertService>();
            advertService.Setup(m => m.findAll()).Returns(adverts);

            var controller = new ClassifiedsController(advertService.Object);
            var result = controller.Index() as ViewResult;
            var list = result.Model as List<Advert>;

            Assert.Equal(3, list.Count());

        }
        [Fact]
        public void TestCategory()
        {
            IEnumerable<Advert> adverts = new List<Advert>
            {
                new Advert{ID=1,Status="SUBMITTED"},
                new Advert{ID=2,Status="APPROVED"},
                new Advert{ID=3,Status="APPROVED"}
            };

            var advertService = new Mock<IAdvertService>();
            advertService.Setup(m => m.findByCategory(It.IsAny<int>())).Returns(adverts);

            var controller = new ClassifiedsController(advertService.Object);
            var result = controller.Category(2) as ViewResult;
            var list = result.Model as List<Advert>;

            Assert.Equal(3, list.Count());

        }
    }
}
