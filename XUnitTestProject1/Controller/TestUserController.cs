using AutoMapper;
using Classifieds.Domain.Model;
using Classifieds.Service;
using Classifieds.Web.Controllers;
using Classifieds.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Classifieds.XUnitTest.Controller
{
    public class TestUserController
    {
        private Mock<IUserService> mockUserService;
        private IMapper mapper;

        public TestUserController()
        {
            mockUserService = new Mock<IUserService>();
            Initialize();
        }
        [Fact]
        public void Delete_ChangedGreaterThanZero()
        {
            mockUserService.Setup(m => m.Delete(It.IsAny<long>())).Returns(1);

            var controller = new UserController(mockUserService.Object, mapper);
            var result = controller.Delete(5) as JsonResult;

            Assert.Equal("Delete Successful!", result.Value);
        }
        [Fact]
        public void Delete_ChangedIsZero()
        {
            mockUserService.Setup(m => m.Delete(It.IsAny<long>())).Returns(0);

            var controller = new UserController(mockUserService.Object,mapper);
            var result = controller.Delete(5) as JsonResult;

            Assert.Equal("Delete Failed. Sorry.", result.Value);
        }
        /// <summary>
        /// Initialize Mapper
        /// </summary>
        private void Initialize()
        {
            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<UserViewModel, User>();
            });

            mapper = configuration.CreateMapper();

        }
    }
}
