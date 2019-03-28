using Classifieds.Domain.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Classifieds.Repository.Impl
{
    public class AdvertRepo : GenericRepo<Advert>, IAdvertRepo
    {
        private ApplicationContext context;

        public AdvertRepo(ApplicationContext context) : base(context)
        {
            this.context = context;
        }
        public override IEnumerable<Advert> findAll()
        {
            return context.Adverts.Include(x => x.Detail);
        }
        public IEnumerable<Advert> findByCategory(int id)
        {
            var adverts = context.Adverts.Where(x => x.MenuID == id)
                .Include(x => x.Detail);

            return adverts;
        }
    }
}
