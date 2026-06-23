using IdentityService.Persistence.Context;
using Microsoft.EntityFrameworkCore;


namespace IdentityService.API.Config
{
    public static class DatabaseConfiguration
    {
        public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppIdentityDbContext>(options =>
            {
                options.UseNpgsql(
                    configuration.GetConnectionString("DefaultConnection"));
            });
            return services;
        }
    }
}
