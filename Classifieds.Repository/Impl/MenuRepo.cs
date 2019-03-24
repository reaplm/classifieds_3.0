using Classifieds.Domain.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Classifieds.Repository.Impl
{
    public class MenuRepo : GenericRepo<Menu>, IMenuRepo
    {
        ApplicationContext context;

        public MenuRepo(ApplicationContext context) : base(context)
        {
            this.context = context;

        }
    }
}
