using Classifieds.Service;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Classifieds.Web.Controllers
{
    

     
    public class ClassifiedsController : Controller
    {
        private IAdvertService advertService;

        public ClassifiedsController(IAdvertService advertService)
        {
            this.advertService = advertService;
        }
        /// <summary>
        /// /Classifieds
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            var adverts = advertService.findAll();

            return View(adverts);
        }
        //Classifieds/Category/id
        public IActionResult Category(int id)
        {
            var adverts = advertService.findByCategory(id);
            return View(adverts);
        }
    }
}
