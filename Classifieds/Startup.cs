using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Classifieds.Domain.Model;
using Classifieds.Repository;
using Classifieds.Repository.Impl;
using Classifieds.Service;
using Classifieds.Service.Impl;
using Classifieds.Web.Model;
using Classifieds.Web.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Classifieds
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //AutoMapper
            var config = new AutoMapper.MapperConfiguration(cfg =>
            {
                cfg.CreateMap<UserViewModel, User>();
                cfg.CreateMap<User, UserViewModel>()
                .ForMember(m => m.RoleList, opts => opts.Ignore());
                cfg.CreateMap<UserDetailViewModel, UserDetail>();
                cfg.CreateMap<UserDetail, UserDetailViewModel>();
                cfg.CreateMap<MenuViewModel, Menu>();
                cfg.CreateMap<AdvertViewModel, Advert>();
                cfg.CreateMap<Advert, AdvertViewModel>()
                    .ForMember(m => m.ParentID, opts => opts.Ignore());
                cfg.CreateMap<AdvertDetailViewModel, AdvertDetail>();
                cfg.CreateMap<AdvertDetail, AdvertDetailViewModel>()
                    .ForMember(m => m.BodySubString, opts => opts.Ignore())
                    .ForMember(m => m.TitleSubString, opts => opts.Ignore());
                cfg.CreateMap<AdPictureViewModel, AdPicture>();
                cfg.CreateMap<AdPicture, AdPictureViewModel>();
                cfg.CreateMap<Category, CategoryViewModel>();
                cfg.CreateMap<Address, AddressViewModel>();
                cfg.CreateMap<AddressViewModel, Address>();
                cfg.CreateMap<RoleViewModel, Role>();
                cfg.CreateMap<LikeViewModel, Like>();
                cfg.CreateMap<Like, LikeViewModel>();
                cfg.CreateMap<NotificationType, NotificationTypeViewModel>();
                cfg.CreateMap<NotificationCategory, NotificationCategoryViewModel>();
            });

            IMapper mapper = config.CreateMapper();
            services.AddSingleton(mapper);

            //Authentication
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = "/Login";
                    options.LogoutPath = "/Login/Signout";
                    //issues a new cookie if user i active before old cookie expires
                    options.SlidingExpiration = true;
                    //logs off the user if the browser is closed or inactive for more than 10 seconds
                    options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
                }
                );

            //session
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                options.Cookie.HttpOnly = true;

            });

            //email settings
            services.AddOptions();
            services.Configure<EmailSettings>(Configuration.GetSection("EmailSettings"));

            services.AddMvc()
                .AddJsonOptions(opts => opts.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

            //MySQL Connection
            var connectionString = Configuration.GetSection("mysqlconnection")["connectionString"];
            
            //Dependency Injection
            services.AddDbContext<ApplicationContext>(options => options.UseMySQL(connectionString));
            services.AddScoped<IMenuService, MenuService>();
            services.AddScoped<IMenuRepo, MenuRepo>();
            services.AddScoped<IAdvertService, AdvertService>();
            services.AddScoped<IAdvertRepo, AdvertRepo>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IUserRepo, UserRepo>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<ICategoryRepo, CategoryRepo>();
            services.AddScoped<IAddressService, AddressService>();
            services.AddScoped<IAddressRepo, AddressRepo>();
            services.AddScoped<IUserDetailService, UserDetailService>();
            services.AddScoped<IUserDetailRepo, UserDetailRepo>();
            services.AddScoped<ILikeRepo, LikeRepo>();
            services.AddScoped<ILikeService, LikeService>();
            services.AddSingleton<IEmailService, EmailService>();
            services.AddScoped<INotificationCategoryRepo, NotificationCategoryRepo>();
            services.AddScoped<INotificationCategoryService, NotificationCategoryService>();
            services.AddScoped<INotificationTypeRepo, NotificationTypeRepo>();
            services.AddScoped<INotificationTypeService, NotificationTypeService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
               app.UseDeveloperExceptionPage();
                //app.UseExceptionHandler("/Error/500");
                //app.UseStatusCodePagesWithReExecute("/Error/{0}");
            }
            else
            {
                app.UseExceptionHandler("/Error/500");
                app.UseStatusCodePagesWithReExecute("/Error/{0}");
            }

            app.UseStaticFiles();

            //Authentication
            app.UseAuthentication();

            //session
            app.UseSession();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");

                routes.MapRoute(
                 "StatusUpdate",
                 "Classifieds/Status/{id}/{status}",
                 new { controller = "Classifieds", Action = "Status" }
                );
                routes.MapRoute(
                "ErrorHandler",
                "Error/{statusCode}",
                new { controller = "Error", Action = "HandleErrorCode" }
               );
                routes.MapRoute(
               "Error500Handler",
               "Error/500",
               new { controller = "Error", Action = "Error500" }
              );
            });

            
        }
    }
}
