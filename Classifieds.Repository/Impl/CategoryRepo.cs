using Classifieds.Domain.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Classifieds.Repository.Impl
{
    public class CategoryRepo : GenericRepo<Category>, ICategoryRepo
    {
        private ApplicationContext appContext;

        public CategoryRepo(ApplicationContext appContext) : base(appContext)
        {
            this.appContext = appContext;
        }
    }
}
