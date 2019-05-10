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
                cfg.CreateMap<UserDetailViewModel, UserDetail>();
                cfg.CreateMap<MenuViewModel, Menu>();
                cfg.CreateMap<AdvertViewModel, Advert>();
                cfg.CreateMap<Advert, AdvertViewModel>()
                    .ForMember(m => m.ParentID, opts => opts.Ignore());
                cfg.CreateMap<AdvertDetailViewModel, AdvertDetail>();
                cfg.CreateMap<AdvertDetail, AdvertDetailViewModel>()
                    .ForMember(m => m.BodySubString, opts => opts.Ignore());
                cfg.CreateMap<AdPictureViewModel, AdPicture>();
                cfg.CreateMap<AdPicture, AdPictureViewModel>();
                cfg.CreateMap<Category, CategoryViewModel>();
            });

            IMapper mapper = config.CreateMapper();
            services.AddSingleton(mapper);

            //Authentication
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = "/Login";
                    options.LogoutPath = "/Login/Signout";
                }
                );

            //session
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(60);
                options.Cookie.HttpOnly = true;
            });

            services.AddMvc()
                .AddJsonOptions(opts => opts.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

            //Dependency Injection
            var connectionString = "server=localhost;port=3306;database=ad_post;uid=developer;password=Sweet05p06=";
            services.AddDbContext<ApplicationContext>(options => options.UseMySQL(connectionString));
            services.AddScoped<IMenuService, MenuService>();
            services.AddScoped<IMenuRepo, MenuRepo>();
            services.AddScoped<IAdvertService, AdvertService>();
            services.AddScoped<IAdvertRepo, AdvertRepo>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IUserRepo, UserRepo>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<ICategoryRepo, CategoryRepo>();

            

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
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

              
            });

            
        }
    }
}
