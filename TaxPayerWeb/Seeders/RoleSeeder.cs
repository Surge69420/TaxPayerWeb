using System;
using Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TaxPayerWeb.Constants;
namespace TaxPayerWeb.Seeders
{
    public static class RoleSeeder
    {
        public static async Task SeedRoles(IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var roles = new string[] { RoleConstants.Admin, RoleConstants.SuperAdmin };
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    var roleToCreate = new IdentityRole(role);
                    await roleManager.CreateAsync(roleToCreate);
                }
            }
        }

        public static async Task SeedUsers(IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var role = RoleConstants.Admin;
            var users = await userManager.Users.ToListAsync();
            foreach (ApplicationUser user in userManager.Users)
            {
                if (user.EmailConfirmed)
                {
                    if (!await userManager.IsInRoleAsync(user, role))
                    {
                        await userManager.AddToRoleAsync(user, role);
                    }
                }
            }
        }
    }
}
