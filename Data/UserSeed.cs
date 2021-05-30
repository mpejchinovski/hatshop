using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using hatshop.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace hatshop.Data
{
    public class UserSeed
    {
        public static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
        {
            if (await roleManager.Roles.AnyAsync())
                return;

            await roleManager.CreateAsync(new IdentityRole("Admin"));
            await roleManager.CreateAsync(new IdentityRole("User"));
        }

        public static async Task SeedAdminAsync(UserManager<ApplicationUser> userManager)
        {
            if (await userManager.FindByNameAsync("admin") == null)
            {
                var defaultAdmin = new ApplicationUser
                {
                    UserName = "admin",
                    Email = "admin@martin.com",
                    FirstName = "Martin",
                    LastName = "Pejchinovski",
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true
                };

                await userManager.CreateAsync(defaultAdmin, "Admin925;");
                await userManager.AddToRoleAsync(defaultAdmin, "Admin");
            }
        }
    }
}