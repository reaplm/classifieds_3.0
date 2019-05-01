using System;
using System.Collections.Generic;
using Xunit;
using Moq;
using Classifieds.Domain.Model;
using Classifieds.Service;
using Classifieds.Controllers;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Classifieds.Web.Models;
using System.Linq.Expressions;

namespace Classifieds.XUnitTest.Controller
{
    public class TestHomeController
    {
        private IMapper mapper;
        private Mock<ICategoryService> mockService;

        public TestHomeController()
        {
            Initialize();
            mockService = new Mock<ICategoryService>();
        }
        /// <summary>
        /// Test { public IActionResult Index() }
        /// </summary>
        [Fact]
        public void Index()
        {

            IEnumerable<Category> categories = new List<Category>
            {
                new Category{ID=1,Name="category 1"},
                new Category{ID=2,Name="category 2" }
            };
            Expression<Func<Category, object>>[] include =
            {
                c => c.SubCategories
            };

            mockService.Setup(m => m.FindAll(It.IsAny<Expression<Func<Category, bool>>>(),
                It.IsAny<Expression<Func<Category, object>>[]>())).Returns(categories);

            var controller = new HomeController(mockService.Object, mapper);

            var result = controller.Index() as ViewResult;
            List<CategoryViewModel> list = result.Model as List<CategoryViewModel>;

            Assert.Equal(2, list.Count);
        }

        private void Initialize()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CategoryViewModel, Category>();
            });

            mapper = config.CreateMapper();
        }
    }
}
