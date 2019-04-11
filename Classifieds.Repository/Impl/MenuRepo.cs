using Classifieds.Domain.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Classifieds.Repository.Impl
{
    public class MenuRepo : GenericRepo<Menu>, IMenuRepo
    {
        ApplicationContext context;

        public MenuRepo(ApplicationContext context) : base(context)
        {
            this.context = context;

        }
        public override IEnumerable<Menu> findAll()
        {
            return context.Menus.Include(x => x.SubMenus);
        }
        public IEnumerable<Menu> findByType(String[] types)
        {
            var menus = context.Menus.Where(m => types.Contains(m.Type))
                .Include(x => x.SubMenus);
            return menus;
        }
        public IEnumerable<Menu> findAll(long parentId)
        {
            //return context.Menus.Find(parentId).SubMenus;
            return context.Menus.Where(x => x.ParentID == parentId).ToList();
        }

    }
}
