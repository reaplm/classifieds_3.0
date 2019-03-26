using Classifieds.Domain.Model;
using Classifieds.Repository;
using Classifieds.Repository.Impl;
using System;
using System.Collections.Generic;
using System.Text;

namespace Classifieds.Service.Impl
{
    public class MenuService : GenericService<Menu>, IMenuService
    {
        private IMenuRepo menuRepository;

        public MenuService(IMenuRepo menuRepository) : base(menuRepository)
        {
            this.menuRepository = menuRepository;
        }
        public IEnumerable<Menu> findByType(String[] types)
        {
            return menuRepository.findByType(types);
        }
    }
}
