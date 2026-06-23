using IdentityService.Domain.Entities;
using IdentityService.Domain.Enum;
using Microsoft.AspNetCore.Identity;

namespace IdentityService.Persistence.Data
{
    public static class RoleSeeder
    {
        public static async Task SeedAsync(RoleManager<ApplicationRole> roleManager)
        {
            foreach (var role in Enum.GetNames(typeof(RoleEnum)))
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(
                        new ApplicationRole
                        {
                            Name = role
                        });
                }
            }
        }
    }
}
