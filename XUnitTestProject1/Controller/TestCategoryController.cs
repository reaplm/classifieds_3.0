using AutoMapper;
using Classifieds.Domain.Enumerated;
using Classifieds.Domain.Model;
using Classifieds.Service;
using Classifieds.Web.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Classifieds.XUnitTest.Controller
{
    public class TestCategoryController
    {
        Mock<ICategoryService> mockCategoryService;
        IMapper mapper;

        public TestCategoryController()
        {
            mockCategoryService = new Mock<ICategoryService>();
            InitMapper();
        }
        /// <summary>
        /// Test for public IActionResult Status(long id, bool approved)
        /// </summary>
        [Fact]
        public void Status()
        {
            Category category = new Category
            {
                ID = 8,
                Name = "Cars",
                Desc = "Cars description",
                Status = (int)EnumTypes.Status.INACTIVE
            };

            
            mockCategoryService.Setup(m => m.Find(It.IsAny<long>())).Returns(category);
            var controller = new CategoryController(mockCategoryService.Object,mapper);

            var result = controller.Status(1, true) as JsonResult;

            Assert.Equal("success", result.Value);
        }
        private void InitMapper()
        {
            var config = new MapperConfiguration(cfg => {
                
            });
            mapper = config.CreateMapper();
        }
    }
}
