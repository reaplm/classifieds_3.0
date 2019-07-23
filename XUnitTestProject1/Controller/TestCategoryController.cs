using AutoMapper;
using Classifieds.Domain.Enumerated;
using Classifieds.Domain.Model;
using Classifieds.Service;
using Classifieds.Web.Controllers;
using Classifieds.Web.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
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
        /// <summary>
        /// Test for public IActionResult Edit(long id)
        /// </summary>
        [Fact]
        public void Edit_GET()
        {
            Category category = new Category
            {
                ID  = 5,
                Name = "Vehicles",
                Desc = "Vehicles Category",
                ParentID = 3,
                Status = 0
            };

            mockCategoryService.Setup(m => m.FindAll(It.IsAny<Expression<Func<Category, bool>>>(),
                It.IsAny<Expression<Func<Category, object>>[]>())).Returns(ParentCategories());

            mockCategoryService.Setup(m => m.Find(It.IsAny<long>())).Returns(category);

            var controller = new CategoryController(mockCategoryService.Object,
                mapper);

            var result = controller.Edit(1) as PartialViewResult;
            var model = result.Model as CategoryViewModel;
            var categories = result.ViewData["Categories"] as List<SelectListItem>;

            Assert.Equal("Vehicles", model.Name);
            Assert.Equal("Vehicles Category", model.Desc);
            Assert.Equal(4, categories.Count);
        }
        /// <summary>
        /// Test for public IActionResult Edit(long id) post
        /// Test when ModelState.Valid = false
        /// </summary>
        [Fact]
        public void Edit_InvalidModelState_POST()
        {
            CategoryViewModel model = new CategoryViewModel
            {
                ID = 8,
                Name = "cars",
                Desc = "cars Category",
                ParentID = 1,
                Label = "cars",
                Status = true
            };


            mockCategoryService.Setup(m => m.FindAll(It.IsAny<Expression<Func<Category, bool>>>(),
                It.IsAny<Expression<Func<Category, object>>[]>())).Returns(ParentCategories());

            var controller = new CategoryController(mockCategoryService.Object,
                mapper);

            controller.ControllerContext.HttpContext = new DefaultHttpContext();
            controller.ModelState.AddModelError("Name", "Name is required");

            var result = controller.Edit(model) as PartialViewResult;
            var categories = result.ViewData["Categories"] as List<SelectListItem>;

            Assert.Equal(200, controller.HttpContext.Response.StatusCode);
            Assert.Equal(4, categories.Count);
            Assert.True(categories[0].Selected);
        }
        /// <summary>
        /// Test for public IActionResult Edit(long id) post
        /// Test when ModelState.Valid = true
        /// </summary>
        [Fact]
        public void Edit_ValidModelState_POST()
        {
            CategoryViewModel model = new CategoryViewModel
            {
                ID = 1,
                Name = "Vehicles",
                Desc = "Vehicles Category",
                ParentID = 2,
                Label = "vehicles",
                Status = true
            };

            var controller = new CategoryController(mockCategoryService.Object,
                mapper);

            controller.ControllerContext.HttpContext = new DefaultHttpContext();
            var result = controller.Edit(model) as JsonResult;

            Assert.Equal("Edit Successful!", result.Value);
            Assert.Equal(201, controller.HttpContext.Response.StatusCode);
        }
        [Fact]
        public void Create_GET()
        {
            mockCategoryService.Setup(m => m.FindAll(It.IsAny<Expression<Func<Category, bool>>>(),
                It.IsAny<Expression<Func<Category, object>>[]>())).Returns(ParentCategories());

            var controller = new CategoryController(mockCategoryService.Object,
                mapper);

            var result = controller.Create() as PartialViewResult;
            List<SelectListItem> categories = (List < SelectListItem > )result.ViewData["Categories"];

            Assert.Equal(4, categories.Count);
        }
        [Fact]
        public void Create_ValidModelState_POST()
        {
           
            CategoryViewModel model = new CategoryViewModel
            {
                ID = 8,
                Name = "cars",
                Desc = "cars Category",
                ParentID = 1,
                Label = "cars",
                Status = true
            };

            var controller = new CategoryController(mockCategoryService.Object,
                mapper);
            controller.ControllerContext.HttpContext = new DefaultHttpContext();

            var result = controller.Create(model) as JsonResult;

            Assert.Equal("Category created successfully!", result.Value);
            Assert.Equal(201, controller.HttpContext.Response.StatusCode);

        }
        [Fact]
        public void Create_InvalidModelState_POST()
        {

            CategoryViewModel model = new CategoryViewModel
            {
                ID = 8,
                Desc = "cars Category",
                ParentID = 1,
                Label = "cars",
                Status = true
            };

            mockCategoryService.Setup(m => m.FindAll(It.IsAny<Expression<Func<Category, bool>>>(),
                It.IsAny<Expression<Func<Category, object>>[]>())).Returns(ParentCategories());

            var controller = new CategoryController(mockCategoryService.Object,
                mapper);
            controller.ControllerContext.HttpContext = new DefaultHttpContext();
            controller.ModelState.AddModelError("Name", "Name is required");

            var result = controller.Create(model) as PartialViewResult;
            var resultModel = result.Model as CategoryViewModel;
            var categories = result.ViewData["Categories"] as List<SelectListItem>;

            Assert.Equal(200, controller.HttpContext.Response.StatusCode);
            Assert.Equal(4, categories.Count);
            Assert.True(categories[0].Selected);
        }
        /// <summary>
        /// Test data
        /// </summary>
        /// <returns></returns>
        private List<Category> ParentCategories()
        {
            return new List<Category>
            {
                new Category{ID = 1, Name = "Vehicles", Label = "vehicles", Status = 1 },
                new Category{ID = 2, Name = "pets", Label = "pets", Status = 1 },
                new Category{ID = 3, Name = "vacation",Label = "vacation", Status = 0 },
                new Category{ID = 4, Name = "services", Label = "services", Status = 0 }
            };
        }
        
        private void InitMapper()
        {
            var config = new MapperConfiguration(cfg => {
                
            });
            mapper = config.CreateMapper();
        }
    }
}
