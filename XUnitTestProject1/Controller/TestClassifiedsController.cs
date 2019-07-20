using AutoMapper;
using Classifieds.Domain.Enumerated;
using Classifieds.Domain.Model;
using Classifieds.Service;
using Classifieds.Web.Controllers;
using Classifieds.Web.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Security.Principal;
using Xunit;

namespace Classifieds.XUnitTest.Controller
{
    public class TestClassifiedsController
    {
        private IMapper mapper;
        private Mock<IAdvertService> mockAdvertService;
        private Mock<ICategoryService> mockCatService;
        private Mock<ClaimsPrincipal> mockClaimsPrincipal;

        public TestClassifiedsController()
        {
            Inialize();
            mockAdvertService = new Mock<IAdvertService>();
            mockCatService = new Mock<ICategoryService>();
            mockClaimsPrincipal = new Mock<ClaimsPrincipal>();
        }
        /// <summary>
        /// Test { public IActionResult Index() }
        /// </summary>
        [Fact]
        public void Index()
        {
            IEnumerable<Advert> adverts = new List<Advert>
            {
                new Advert{ID=1,Status="SUBMITTED"},
                new Advert{ID=2,Status="APPROVED"},
                new Advert{ID=3,Status="APPROVED"}
            };

            var menuService = new Mock<IMenuService>();
            var advertService = new Mock<IAdvertService>();
            advertService.Setup(m => m.FindAll(It.IsAny<Expression<Func<Advert,bool>>>(),
                It.IsAny<Expression<Func<Advert,object>>[]>())).Returns(adverts);

            var controller = new ClassifiedsController(advertService.Object,
                mockCatService.Object, mapper);
            var result = controller.Index() as ViewResult;
            var list = result.Model as List<AdvertViewModel>;

            Assert.Equal(3, list.Count());

        }
        /// <summary>
        /// Test { public IActionResult Category(int id) }
        /// //Classifieds/Category/id
        /// </summary>
        [Fact]
        public void Category()
        {
            IEnumerable<Advert> adverts = new List<Advert>
            {
                new Advert{ID=1,Status="SUBMITTED"},
                new Advert{ID=2,Status="APPROVED"},
                new Advert{ID=3,Status="APPROVED"}
            };

            var menuService = new Mock<IMenuService>();
            var advertService = new Mock<IAdvertService>();
            advertService.Setup(m => m.FindByCategory(It.IsAny<int>())).Returns(adverts);

            var controller = new ClassifiedsController(advertService.Object,
                mockCatService.Object, mapper);
            var result = controller.Category(2) as ViewResult;
            var list = result.Model as List<Advert>;

            Assert.Equal(3, list.Count());

        }
        /// <summary>
        /// Test { public IActionResult Create() }
        /// Mocks authentication, HttpContext
        /// </summary>
        [Fact]
        public void Create_GET()
        {
            IEnumerable<Category> categories = GetCatList();

            //Setup to allow query of authenticated user
            //  (HttpContext.User.Claims.FirstOrDefault(u => u.Type == "UserId").Value)
            var mockIdentity = new GenericIdentity("User");
            mockIdentity.AddClaim(new Claim("UserId", "1"));
            var principal = new GenericPrincipal(mockIdentity, null);


            var mockHttpContext = new Mock<HttpContext>();
            mockHttpContext.Setup(m => m.User).Returns(principal);

            mockCatService.Setup(m => m.FindAll(It.IsAny< Expression<Func<Category, bool>>>(),
                It.IsAny< Expression<Func<Category, object>>[]>()))
                .Returns(categories);

            var controller = new ClassifiedsController(mockAdvertService.Object,
                 mockCatService.Object, mapper);

            controller.ControllerContext.HttpContext = mockHttpContext.Object;

            var result = controller.Create() as ViewResult;
            var categoryList = result.ViewData["Categories"] as IEnumerable<SelectListItem>;
            var subCategories = result.ViewData["SubCategories"] as IEnumerable<SelectListItem>;


            Assert.IsType<AdvertViewModel>(result.Model);
            Assert.Equal(3, categoryList.Count());
            Assert.Empty(subCategories);
        }
        /// <summary>
        /// Test { public IActionResult Create(AdvertViewModel model) }
        /// Test when ModelState.IsValid = false
        /// </summary>
        [Fact]
        public void Create_InvalidModelState_POST()
        {
            var categories = GetCatList();
            var subcategories = GetSubCatList();
            AdvertViewModel model = new AdvertViewModel
            {
                CategoryID = 2,
                ParentID = 1
            };

            Expression<Func<Category, bool>> where = c => c.ParentID == null;
            Expression<Func<Category, bool>> subwhere = x => x.ParentID == 1;

             

            var controller = new ClassifiedsController(mockAdvertService.Object,
                mockCatService.Object, mapper);
            controller.ModelState.AddModelError("category", "please select category");//set ModelState to be invalid

            var result = controller.Create(model) as ViewResult;
            var list = result.ViewData["Categories"] as IEnumerable<SelectListItem>;
            var subs = result.ViewData["SubCategories"] as IEnumerable<SelectListItem>;

            Assert.Equal(3, list.Count());
            Assert.Equal(2, subs.Count());
            Assert.True(subs.FirstOrDefault(x => x.Value.Equals("2")).Selected);
            Assert.True(list.FirstOrDefault(x => x.Value.Equals("1")).Selected);
        }
        /// <summary>
        /// Test { public IActionResult Create(AdvertViewModel model) }
        /// Create a new advert and redirect on success
        /// </summary>
        [Fact]
        public void CreateValidModelState_POST()
        {
            AdvertViewModel model = GetAdvert();

            var controller = new ClassifiedsController(mockAdvertService.Object,
                mockCatService.Object, mapper);
            var result = controller.Create(model) as RedirectToActionResult;

            Assert.Equal("Index", result.ActionName);
            Assert.Equal("Classifieds", result.ControllerName);
            mockAdvertService.Verify(m => m.Create(It.IsAny<Advert>()), Times.Once());
            mockAdvertService.Verify(m => m.Save(), Times.Once());

        }
        /// <summary>
        /// Test { public IActionResult Detail(long id) }
        /// </summary>
        [Fact]
        public void Detail()
        {
            Advert advert = mapper.Map<Advert>(GetAdvert());

            mockAdvertService.Setup(m => m.Find(It.IsAny<long>(), 
                It.IsAny<Expression<Func<Advert, object>>[]>()))
                .Returns(advert);

            var controller = new ClassifiedsController(mockAdvertService.Object,
                mockCatService.Object, mapper);

            var result = controller.Detail(3) as PartialViewResult;
            var model = result.Model as AdvertViewModel;

            Assert.Equal("pearl@email", model.Detail.Email);
            Assert.Equal("Black Toyota for sale", model.Detail.Title);
            Assert.Equal(2, model.Detail.GroupCount);
            Assert.Equal(2, model.Detail.AdPictures.Count());


        }
        /// <summary>
        /// Test { public IActionResult Edit(long id) }
        /// </summary>
        [Fact]
        public void Edit_GET()
        {
            var categories = GetCatList();
            var subCategories = GetSubCatList();

            var queue = new Queue<IEnumerable<Category>>();
            queue.Enqueue(categories);
            queue.Enqueue(subCategories);

            mockAdvertService.Setup(m => m.Find(It.IsAny<long>(), It.IsAny< Expression<Func<Advert, object>>[]>()))
                .Returns(mapper.Map<Advert>(GetAdvert()));
            mockCatService.Setup(m => m.FindAll(It.IsAny<Expression<Func<Category, bool>>>(),
                It.IsAny<Expression<Func<Category, object>>[]>()))
                .Returns(queue.Dequeue);
            mockCatService.Setup(m => m.FindAll(It.IsAny<Expression<Func<Category, bool>>>(),
                It.IsAny<Expression<Func<Category, object>>[]>()))
                .Returns(queue.Dequeue);

            var controller = new ClassifiedsController(mockAdvertService.Object,
                mockCatService.Object, mapper);


            var result = controller.Edit(4L) as PartialViewResult;
            var model = result.Model as AdvertViewModel;
            var subs = result.ViewData["SubCategories"] as IEnumerable<SelectListItem>;
            var cats = result.ViewData["Categories"] as IEnumerable<SelectListItem>;

            Assert.Equal(2, model.ParentID);
            Assert.Equal(6, model.CategoryID);
            Assert.Equal(2, subs.Count());
            Assert.Equal(3, cats.Count());
        }
        /// <summary>
        /// Test { public IActionResult Edit(AdvertViewModel model) }
        /// ModelState.Valid = false
        /// </summary>
        [Fact]
        public void EditInvalidModelState_POST()
        {
            var categories = GetCatList();
            var subCategories = GetSubCatList();

            var queue = new Queue<IEnumerable<Category>>();
            queue.Enqueue(categories);
            queue.Enqueue(subCategories);

            mockCatService.Setup(m => m.FindAll(It.IsAny<Expression<Func<Category, bool>>>(),
                It.IsAny<Expression<Func<Category, object>>[]>()))
                .Returns(queue.Dequeue);
            mockCatService.Setup(m => m.FindAll(It.IsAny<Expression<Func<Category, bool>>>(),
                It.IsAny<Expression<Func<Category, object>>[]>()))
                .Returns(queue.Dequeue);

            var model = GetAdvert();

            var controller = new ClassifiedsController(mockAdvertService.Object,
                mockCatService.Object, mapper);
            controller.ControllerContext.HttpContext = new DefaultHttpContext();

            //create ModelState.IsValid = false
            controller.ModelState.AddModelError("Location","Location is required");

            var result = controller.Edit(model) as PartialViewResult;
            var resultModel = result.Model as AdvertViewModel;
            var subs = result.ViewData["SubCategories"] as IEnumerable<SelectListItem>;
            var cats = result.ViewData["Categories"] as IEnumerable<SelectListItem>;
            var status = controller.HttpContext.Response.StatusCode;     

            Assert.Equal(6, resultModel.CategoryID);
            Assert.Equal(2, subs.Count());
            Assert.Equal(3, cats.Count());
            Assert.Equal(200, status);
        }
        /// <summary>
        /// Test { public IActionResult Edit(AdvertViewModel model) }
        /// ModelState.Valid = true
        /// </summary>
        [Fact]
        public void EditValidModelState_POST_If()
        {
            mockAdvertService.Setup(m => m.Update(It.IsAny<Advert>(), It.IsAny<Object[]>(),
                It.IsAny<string[]>())).Returns(1);
            mockAdvertService.Setup(m => m.RemoveAllPictures(It.IsAny<long>()))
                .Returns(2);

            var model = GetAdvert();

            var controller = new ClassifiedsController(mockAdvertService.Object,
                mockCatService.Object, mapper);
            controller.ControllerContext.HttpContext = new DefaultHttpContext();

            var result = controller.Edit(model) as JsonResult;
            var status = controller.HttpContext.Response.StatusCode;

            Assert.Equal(201, status);
            Assert.Equal("Ádvert Updated!", result.Value);

        }
        /// <summary>
        /// Test { public IActionResult Edit(AdvertViewModel model) }
        /// ModelState.Valid = true
        /// </summary>
        [Fact]
        public void EditValidModelState_POST_Else()
        {
            mockAdvertService.Setup(m => m.Update(It.IsAny<Advert>(), It.IsAny<Object[]>(),
                It.IsAny<string[]>())).Returns(0);
            mockAdvertService.Setup(m => m.RemoveAllPictures(It.IsAny<long>()))
                .Returns(2);

            var model = GetAdvert();

            var controller = new ClassifiedsController(mockAdvertService.Object,
                mockCatService.Object, mapper);
            controller.ControllerContext.HttpContext = new DefaultHttpContext();

            var result = controller.Edit(model) as JsonResult;
            var status = controller.HttpContext.Response.StatusCode;

            Assert.Equal(201, status);
            Assert.Equal("Process completed but no rows were affected", result.Value);

        }
        /// <summary>
        /// Test for public IActionResult Status(long id, bool approved)
        /// </summary>
        [Fact]
        public void Status()
        {
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
                Location = "Gaborone"
            };

            Advert advert = new Advert
            {
                ID = 1,
                Status = EnumTypes.AdvertStatus.SUBMITTED.ToString(),
                Detail = advertDetail
            };
            mockAdvertService.Setup(m => m.Find(It.IsAny<long>())).Returns(advert);
            var controller = new ClassifiedsController(mockAdvertService.Object,
                mockCatService.Object, mapper);

            var result = controller.Status(1, true) as JsonResult;

            Assert.Equal("success", result.Value);
        }
        /// <summary>
        /// Initialize AutoMapper
        /// </summary>
        private void Inialize()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<AdvertViewModel, Advert>();
                cfg.CreateMap<Advert, AdvertViewModel>()
                    .ForMember(m => m.ParentID, opts => opts.Ignore());
                cfg.CreateMap<AdvertDetailViewModel, AdvertDetail>();
                cfg.CreateMap<AdvertDetail, AdvertDetailViewModel>()
                    .ForMember(m => m.BodySubString, opts =>opts.Ignore());
                cfg.CreateMap<AdPictureViewModel, AdPicture>();
                cfg.CreateMap<AdPicture, AdPictureViewModel>();
            });

            mapper = config.CreateMapper();
        }
        /// <summary>
        /// Data (Menu SelectList)
        /// </summary>
        /// <returns></returns>
        private IEnumerable<Category> GetCatList()
        {

            IEnumerable<Category> categories = new List<Category>
            {
                new Category{ID=1,Name="cat1"},
                new Category{ID=2,Name="cat2"},
                new Category{ID=3,Name="cat3"},
            };

            return categories;
        }
        /// <summary>
        /// Data (SubMenu SelectList)
        /// </summary>
        /// <returns></returns>
        private IEnumerable<Category> GetSubCatList()
        {

            IEnumerable<Category> categories = new List<Category>
            {
                new Category{ID=1,Name="subcat1"},
                new Category{ID=2,Name="subcat2"}
            };

            return categories;
        }
        /// <summary>
        /// Create Advert object
        /// </summary>
        /// <returns></returns>
        private AdvertViewModel GetAdvert()
        {

            var category = new CategoryViewModel { ID = 6, Name = "subcat2", ParentID=2 };
           
            AdPictureViewModel picture1 = new AdPictureViewModel
            {
                ID = 1,
                Uuid = "0b83b507-8c11-4c0e-96d2-5fd773d525f7",
                CdnUrl = "https://ucarecdn.com/0b83b507-8c11-4c0e-96d2-5fd773d525f7/",
                Name = "about me sample 3.PNG",
                Size = 135083
            };
            AdPictureViewModel picture2 = new AdPictureViewModel
            {
                ID = 2,
                Uuid = "c1df9f17-61ad-450a-87f9-d846c312dae0",
                CdnUrl = "https://ucarecdn.com/c1df9f17-61ad-450a-87f9-d846c312dae0/",
                Name = "about me sample 4.PNG",
                Size = 146888
            };
            List<AdPictureViewModel> pictures = new List<AdPictureViewModel> { picture1, picture2 };

            AdvertDetailViewModel advertDetail = new AdvertDetailViewModel
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

            AdvertViewModel advert = new AdvertViewModel
            {
                ID = 8,
                Status = EnumTypes.AdvertStatus.SUBMITTED.ToString(),
                CategoryID = 6,
                Detail = advertDetail,
                Category = category
            };

            return advert;
        }

        /// <summary>
        /// Mock Authentication Service
        /// </summary>
        /// <returns></returns>
        private IServiceProvider ServiceProviderMock()
        {
            var authServiceMock = new Mock<IAuthenticationService>();
            var urlHelperFactory = new Mock<IUrlHelperFactory>();
            var dataDictionaryFactory = new Mock<ITempDataDictionaryFactory>();

            var serviceProviderMock = new Mock<IServiceProvider>();
            serviceProviderMock.Setup(m => m.GetService(typeof(IAuthenticationService)))
                .Returns(authServiceMock.Object);
            serviceProviderMock.Setup(m => m.GetService(typeof(Microsoft.AspNetCore.Mvc.Routing.IUrlHelperFactory)))
                .Returns(urlHelperFactory.Object);
            serviceProviderMock.Setup(m => m.GetService(typeof(ITempDataDictionaryFactory)))
                .Returns(dataDictionaryFactory.Object);

            return serviceProviderMock.Object;
        }
    }
}
