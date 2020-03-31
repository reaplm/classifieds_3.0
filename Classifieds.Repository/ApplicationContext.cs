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
        public virtual DbSet<AdvertDetail> AdvertDetails { set; get; }
        public virtual DbSet<User> Users { set; get; }
        public virtual DbSet<UserDetail> UserDetails { set; get; }
        public virtual DbSet<AdPicture> AdPictures { set; get; }
        public virtual DbSet<Category> Categories { set; get; }
        public virtual DbSet<Address> Addresses { set; get; }
        public virtual DbSet<Role> Roles { set; get; }
        public virtual DbSet<Like> Likes { set; get; }
        public virtual DbSet<NotificationCategory> NotificationCategories { set; get; }
        public virtual DbSet<NotificationType> NotificationTypes { set; get; }

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
