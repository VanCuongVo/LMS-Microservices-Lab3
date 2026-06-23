using IdentityService.Domain.Entities;
using IdentityService.Domain.Enum;
using IdentityService.Persistence.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace IdentityService.Persistence.Data
{
    public static class DatabaseSeeder
    {
        public static async Task SeedAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AppIdentityDbContext>();
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<AppIdentityDbContext>>();

            logger.LogInformation("Applying database migrations...");
            await context.Database.MigrateAsync();
            logger.LogInformation("Database migrations applied successfully.");

            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
            await SeedRolesAsync(roleManager);

            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            await SeedTestUsersAsync(userManager);
        }

        private static async Task SeedRolesAsync(RoleManager<ApplicationRole> roleManager)
        {
            foreach (var role in Enum.GetNames(typeof(RoleEnum)))
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new ApplicationRole { Name = role });
                }
            }
        }

        private static async Task SeedTestUsersAsync(UserManager<ApplicationUser> userManager)
        {
            var testUsers = new[]
            {
                new
                {
                    UserName = "admin",
                    Email = "admin@lms.com",
                    FullName = "System Admin",
                    Password = "Admin@123",
                    Role = RoleEnum.Admin.ToString()
                },
                new
                {
                    UserName = "student",
                    Email = "student@lms.com",
                    FullName = "Jane Student",
                    Password = "Student@123",
                    Role = RoleEnum.Student.ToString()
                }
            };

            foreach (var user in testUsers)
            {
                if (await userManager.FindByEmailAsync(user.Email) != null)
                    continue;

                var appUser = new ApplicationUser
                {
                    UserName = user.UserName,
                    Email = user.Email,
                    FullName = user.FullName
                };

                var result = await userManager.CreateAsync(appUser, user.Password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(appUser, user.Role);
                }
            }
        }
    }
}
