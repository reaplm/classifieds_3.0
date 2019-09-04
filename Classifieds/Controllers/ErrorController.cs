using Classifieds.Models;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Classifieds.Web.Controllers
{
    public class ErrorController : Controller
    {
        /// <summary>
        /// Internal Error(500) Exception handler
        /// [Route("/Error/500")]
        /// </summary>
        /// <returns></returns>
        public IActionResult Error500()
        {
            var exceptionFeature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();

            if (exceptionFeature != null)
            {
                ViewBag.ErrorMessage = exceptionFeature.Error.Message;
                ViewBag.RouteOfException = exceptionFeature.Path;
            }

            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier, StatusCode = HttpContext.Response.StatusCode });

        }
        /// <summary>
        /// Other Error codes
        /// [Route("/Error/{statusCode}")]
        /// </summary>
        /// <returns></returns>
        public IActionResult HandleErrorCode(int statusCode)
        {
            var statusCodeData = HttpContext.Features.Get<IStatusCodeReExecuteFeature>();

            switch(statusCode){
                case 404:
                    ViewBag.ErrorMessage = "Sorry the page you requested could not be found";
                    ViewBag.RouteOfException = statusCodeData?.OriginalPath;
                    break;
                case 500:
                    ViewBag.ErrorMessage = "Sorry something went wrong on the server";
                    ViewBag.RouteOfException = statusCodeData?.OriginalPath;
                    break;
                default:
                    ViewBag.ErrorMessage = "Sorry an error occured while processing your request";
                    ViewBag.RouteOfException = statusCodeData?.OriginalPath;
                    break;
            }
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier, StatusCode = HttpContext.Response.StatusCode });

        }
    }
}
