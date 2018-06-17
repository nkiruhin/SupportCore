using Microsoft.AspNetCore.Identity;
using SupportCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SupportCore.App.Classes
{
    public class IdentityDataInitializer
    {
        public static void SeedData(UserManager<User> userManager,RoleManager<IdentityRole> roleManager)
        {
            SeedRoles(roleManager);
            SeedUsers(userManager);
        }

        public static void SeedUsers(UserManager<User> userManager)
        {
            if (userManager.FindByNameAsync("admin").Result == null)
            {
                User user = new User
                {
                    UserName = "admin",
                    Email = "admin@localhost"
                };
                IdentityResult result = userManager.CreateAsync
                (user, "Password123").Result;

                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user,"Администратор").Wait();
                }
            }
        }

        public static void SeedRoles(RoleManager<IdentityRole> roleManager)
        {
            if (!roleManager.RoleExistsAsync("Администратор").Result)
            {
                roleManager.CreateAsync(new IdentityRole("Администратор"));
            }
            if (!roleManager.RoleExistsAsync("Менеджер").Result)
            {
                roleManager.CreateAsync(new IdentityRole("Менеджер"));
            }
            if (!roleManager.RoleExistsAsync("Пользователь").Result)
            {
                roleManager.CreateAsync(new IdentityRole("Пользователь"));
            }
            if (!roleManager.RoleExistsAsync("Сотрудник").Result)
            {
                roleManager.CreateAsync(new IdentityRole("Сотрудник"));
            }
        }
    }
}
