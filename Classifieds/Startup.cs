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
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
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
            //services.AddAutoMapper();
            var config = new AutoMapper.MapperConfiguration(cfg =>
            {
                cfg.CreateMap<UserViewModel, User>();
            });

            IMapper mapper = config.CreateMapper();
            services.AddSingleton(mapper);

            services.AddMvc();

            //Dependency Injection
            var connectionString = "server=localhost;port=3306;database=ad_post;uid=developer;password=Sweet05p06=";
            services.AddDbContext<ApplicationContext>(options => options.UseMySQL(connectionString));
            services.AddScoped<IMenuService, MenuService>();
            services.AddScoped<IMenuRepo, MenuRepo>();
            services.AddScoped<IAdvertService, AdvertService>();
            services.AddScoped<IAdvertRepo, AdvertRepo>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IUserRepo, UserRepo>();

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

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");

              
            });
        }
    }
}
