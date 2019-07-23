using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Classifieds.Domain.Enumerated;
using Classifieds.Domain.Model;
using Classifieds.Service;
using Classifieds.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.OData.Query.SemanticAst;

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
        public IActionResult Create()
        {
            ViewBag.Categories = ParentCategories(null);
            return PartialView();
        }
        [HttpPost]
        public IActionResult Create(CategoryViewModel model)
        {
            if (ModelState.IsValid)
            {
                categoryService.Create(mapper.Map<Category>(model));
                categoryService.Save();

                HttpContext.Response.StatusCode = (int)HttpStatusCode.Created;
                return new JsonResult("Category created successfully!");
            }

            ViewBag.Categories = ParentCategories(model.ParentID);
            HttpContext.Response.StatusCode = (int)HttpStatusCode.OK;
            return PartialView(model);
        }
        /// <summary>
        /// Edit category
        /// </summary>
        /// <param name="id">id of the category to edit</param>
        /// <returns></returns>
        [Authorize]
        public IActionResult Edit(long id)
        {
            CategoryViewModel model = mapper.Map<CategoryViewModel>
                (categoryService.Find(id));

            ViewBag.Categories = ParentCategories(null);

            return PartialView(model);
        }
        /// <summary>
        /// Edit category submit 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult Edit(CategoryViewModel model)
        {
            if (ModelState.IsValid)
            {
                model.Label = model.Name.Replace(" ", "");
                categoryService.Update(mapper.Map<Category>(model));
                categoryService.Save();

                HttpContext.Response.StatusCode = (int)HttpStatusCode.Created ;
                return new JsonResult ( "Edit Successful!" );
            }

            ViewBag.Categories = ParentCategories(model.ParentID);
            HttpContext.Response.StatusCode = (int)HttpStatusCode.OK;
            return PartialView(model);
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
        /// <summary>
        /// Fetch all parent categories to pupulate select list
        /// </summary>
        /// <param name="selectedIndex"></param>
        /// <returns></returns>
        private IEnumerable<SelectListItem> ParentCategories(long? selectedIndex)
        {
            List<SelectListItem> selectItems = new List<SelectListItem>();
            Expression<Func<Category, bool>> where = c => c.ParentID == null;
            var categories = categoryService.FindAll(where, null);


            foreach (var category in categories)
            {
                selectItems.Add(
                    new SelectListItem
                    {
                        Text = category.Name,
                        Value = category.ID.ToString(),
                        Selected = category.ID == selectedIndex ? true: false
                    }
                );
            }

            return selectItems;
        }
    }
}