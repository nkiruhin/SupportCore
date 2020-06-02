using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CustomConfigurationProvider;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SupportCore.App.Classes;
using SupportCore.App.Interfaces;
using SupportCore.Models;
using SupportCore.ViewModels;

namespace SupportCore
{
    public class Startup
    {
        //public Startup(IConfiguration configuration)
        //{
        //    var builder = new ConfigurationBuilder()
        //     .SetBasePath(Directory.GetCurrentDirectory())
        //     .AddJsonFile("appsettings.json");
        //    var connectionStringConfig = builder.Build();

        //    var config = new ConfigurationBuilder()
        //        .SetBasePath(Directory.GetCurrentDirectory())
        //        // Add "appsettings.json" to bootstrap EF config.
        //        .AddJsonFile("appsettings.json")
        //        // Add the EF configuration provider, which will override any
        //        // config made with the JSON provider.
        //        .AddEntityFrameworkConfig(options =>
        //            options.UseSqlServer(connectionStringConfig.GetConnectionString(
        //                "DefaultConnection"))
        //        )
        //        .Build();
        //    //Configuration = configuration;
        //    Configuration = config;
        //}
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            //services.AddMvc();

            services.AddDbContext<Context>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            services.AddDbContext<IdentityContext>(options => options.UseSqlServer(Configuration.GetConnectionString("IdentityConnection")));
            services.AddIdentity<User, IdentityRole>(options =>
            {
                // Password settings
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 6;
                //options.Password.RequiredUniqueChars = 2;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                // Lockout settings
                options.Lockout.AllowedForNewUsers = true;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromHours(8);
                options.Lockout.MaxFailedAccessAttempts = 5;
            })
            .AddEntityFrameworkStores<IdentityContext>()
            .AddDefaultTokenProviders();
            services.ConfigureApplicationCookie(options =>
            {
                // Cookie settings
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromHours(7);
                // If the LoginPath isn't set, ASP.NET Core defaults 
                // the path to /Account/Login.
                options.LoginPath = "/Account/Login";
                // If the AccessDeniedPath isn't set, ASP.NET Core defaults 
                // the path to /Account/AccessDenied.
                options.AccessDeniedPath = "/Account/AccessDenied";
                options.SlidingExpiration = true;
            });
            services.AddMvc(config =>
            {
                var policy = new AuthorizationPolicyBuilder()
                                 .RequireAuthenticatedUser()
                                 .Build();
                config.Filters.Add(new AuthorizeFilter(policy));
            })
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.Configure<EmailConfig>(Configuration.GetSection("Email"));
            services.Configure<AppSetting>(Configuration.GetSection("AppSetting"));
            services.Configure<SipConfig>(Configuration);
            services.Configure<EmailConfig>(Configuration);
            services.AddTransient<IEmailService, EmailService>();
            services.AddSignalR();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                //app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseSignalR(routes =>
            {
                routes.MapHub<SignalrHub>("/hub");
            });
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
