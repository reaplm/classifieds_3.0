using Classifieds.Domain.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Classifieds.XUnitTest
{
        public class ApplicationContext : DbContext
        {
            public virtual DbSet<Menu> Menus { set; get; }

            public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
            {

            }
            protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
                //modelBuilder.ApplyConfiguration.Remove<PluralizingTableNameConvention>();
                base.OnModelCreating(modelBuilder);
            }
        }
    }

}
