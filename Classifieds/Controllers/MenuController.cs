using Classifieds.Domain.Model;
using Classifieds.Service;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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
        /// <summary>
        /// classifieds/SubMenus/2
        /// Fetch all submenus with parentId {id}
        /// </summary>
        /// <param name="id">parent id</param>
        /// <returns></returns>
        public IActionResult SubMenus(int id)
        {
            Expression<Func<Menu, bool>> whereCondition = m => m.ParentID == id;

            var subMenus = menuService.FindAll(whereCondition, null) as List<Menu>;

            return Ok(new { results = subMenus });

        }
    }
}
