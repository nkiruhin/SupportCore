using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CustomConfigurationProvider;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SupportCore.App.Classes;
using SupportCore.Models;

namespace SupportCore
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateWebHostBuilder(args).Build();

                using (var scope = host.Services.CreateScope())
                {
                    var serviceProvider = scope.ServiceProvider;
                    var icontext = serviceProvider.GetRequiredService<IdentityContext>();
                    icontext.Database.Migrate();
                    try
                    {
                        var userManager = serviceProvider.GetRequiredService<UserManager<User>>();

                        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                        IdentityDataInitializer.SeedData(userManager, roleManager);
                    }
                catch (Exception ex)
                {
                    var logger = serviceProvider.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred while seeding the database.");
                    throw ex;
                }
            }
            host.Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    config.SetBasePath(Directory.GetCurrentDirectory());
                    var connectionString = config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true).Build();
                    config.AddEntityFrameworkConfig(options =>
                                options.UseSqlServer(connectionString.GetConnectionString("DefaultConnection")));
                    //config.AddCommandLine(args);
                })
                .UseStartup<Startup>();
    }
}
