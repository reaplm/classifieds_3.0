using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using Classifieds.Domain.Enumerated;
using Classifieds.Domain.Model;
using Classifieds.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Classifieds.Web.Controllers
{
    public class CategoryController : Controller
    {
        private ICategoryService categoryService;
        private IMapper mapper;

        public CategoryController(ICategoryService categoryService, IMapper mapper)
        {
            this.categoryService = categoryService;
            this.mapper = mapper;
        }
        /// <summary>
        /// Fetch all subcategories where ParentID is equal to id
        /// /Category/SubCategories/id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IActionResult SubCategories(long id)
        {
            Expression<Func<Category, bool>> where = c => c.ParentID == id;

            IEnumerable<Category> subCategories = categoryService.FindAll(where, null);

            return Ok(new { results = subCategories });
        }
        /// <summary>
        /// Update status of advert (approve/reject)
        /// url: /classifieds/Status/id/approved
        /// </summary>
        /// <param name="id">Advert id</param>
        /// <param name="approved">status</param>
        /// <returns></returns>
        [Authorize]
        public IActionResult Status(long id, bool active)
        {
            var category = categoryService.Find(id);

            if (active)
            {
                category.Status = (int)EnumTypes.Status.ACTIVE;

            }
            else
            {
                category.Status = (int)EnumTypes.Status.INACTIVE;
            }

            categoryService.Update(category);
            categoryService.Save();

            return new JsonResult("success");
        }
    }
}