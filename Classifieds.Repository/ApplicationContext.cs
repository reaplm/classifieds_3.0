using Classifieds.Domain.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;


namespace Classifieds.Repository
{
    public class ApplicationContext : DbContext
    {
        public virtual DbSet<Menu> Menus { set; get; }
        public virtual DbSet<Advert> Adverts { set; get; }

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
