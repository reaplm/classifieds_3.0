using Classifieds.Models;
using Classifieds.Web.Controllers;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Text;
using Xunit;

namespace Classifieds.XUnitTest.Controller
{
    public class TestErrorController
    {
        public TestErrorController()
        {
        }
        /// <summary>
        /// Test Error 500
        /// </summary>
        [Fact]
        public void Error500()
        {
            var controller = new ErrorController();
            controller.ControllerContext.HttpContext = new DefaultHttpContext();
            controller.HttpContext.Response.StatusCode = ((int)HttpStatusCode.InternalServerError);
            controller.HttpContext.Features.Set<IExceptionHandlerPathFeature>(
                new ExceptionHandlerFeature
                {
                    Error = new Exception("MySQL Connection error"),
                    Path = "/Home"
                });

            var result = controller.Error500() as ViewResult;
            var model = result.Model as ErrorViewModel;

            Assert.Equal(500, model.StatusCode);
            Assert.True(model.ShowRequestId);
            Assert.Equal("MySQL Connection error", result.ViewData["ErrorMessage"]);
            Assert.Equal("/Home", result.ViewData["RouteOfException"]);

        }
        /// <summary>
        /// Test Error 404
        /// </summary>
        [Fact]
        public void HandleErrorCode404()
        {
            var controller = new ErrorController();
            controller.ControllerContext.HttpContext = new DefaultHttpContext();
            controller.HttpContext.Response.StatusCode = ((int)HttpStatusCode.NotFound);
            controller.HttpContext.Features.Set< IStatusCodeReExecuteFeature > (
                new StatusCodeReExecuteFeature
                {
                    OriginalPath = "/Classifieds/Create",

                });

            var result = controller.HandleErrorCode(404) as ViewResult;
            var model = result.Model as ErrorViewModel;

            Assert.Equal(404, model.StatusCode);
            Assert.True(model.ShowRequestId);
            Assert.Equal("Sorry the page you requested could not be found", result.ViewData["ErrorMessage"]);
            Assert.Equal("/Classifieds/Create", result.ViewData["RouteOfException"]);

        }
        /// <summary>
        /// Test error 500
        /// </summary>
        [Fact]
        public void HandleErrorCode500()
        {
            var controller = new ErrorController();
            controller.ControllerContext.HttpContext = new DefaultHttpContext();
            controller.HttpContext.Response.StatusCode = ((int)HttpStatusCode.InternalServerError);
            controller.HttpContext.Features.Set<IStatusCodeReExecuteFeature>(
                new StatusCodeReExecuteFeature
                {
                    OriginalPath = "/Classifieds/Create",

                });

            var result = controller.HandleErrorCode(500) as ViewResult;
            var model = result.Model as ErrorViewModel;

            Assert.Equal(500, model.StatusCode);
            Assert.True(model.ShowRequestId);
            Assert.Equal("Sorry something went wrong on the server", result.ViewData["ErrorMessage"]);
            Assert.Equal("/Classifieds/Create", result.ViewData["RouteOfException"]);

        }
    }
}
