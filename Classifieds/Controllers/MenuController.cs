using Classifieds.Domain.Model;
using Classifieds.Service;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Classifieds.Web.Controllers
{
    public class MenuController : Controller
    {
        private IMenuService menuService;

        public MenuController(IMenuService menuService)
        {
            this.menuService = menuService;
        }
        //classifieds/SubMenus/2
        public IActionResult SubMenus(int id)
        {
            var subMenus = menuService.findAll(id) as List<Menu>;

            return Ok(new { results = subMenus });
        }
    }
}
