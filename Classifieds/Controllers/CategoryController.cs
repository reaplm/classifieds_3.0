using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using Classifieds.Domain.Model;
using Classifieds.Service;
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
    }
}