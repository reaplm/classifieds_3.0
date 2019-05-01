using Classifieds.Domain.Model;
using Classifieds.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace Classifieds.Service.Impl
{
    public class CategoryService : GenericService<Category>, ICategoryService
    {
        private ICategoryRepo categoryRepo;

        public CategoryService(ICategoryRepo categoryRepo) : base(categoryRepo)
        {
            this.categoryRepo = categoryRepo;
        }
    }
}
