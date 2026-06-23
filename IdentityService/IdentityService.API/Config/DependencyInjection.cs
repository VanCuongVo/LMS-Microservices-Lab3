using IdentityService.Application.Features;
using IdentityService.Application.Interfaces;
using IdentityService.Application.IServices;
using IdentityService.Domain.Entities;
using IdentityService.Domain.IRepository;
using IdentityService.Domain.IUnitOfWork;
using IdentityService.Infrastructure.Services;
using IdentityService.Persistence.Context;
using IdentityService.Persistence.Repository;
using Microsoft.AspNetCore.Identity;


namespace IdentityService.API.Config
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDependencyInjection(this IServiceCollection services)
        {
            services
    .AddIdentity<ApplicationUser, ApplicationRole>()
    .AddEntityFrameworkStores<AppIdentityDbContext>()
    .AddDefaultTokenProviders();
            services.AddScoped<IJwtService, JwtService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped<ILogService, LogService>();
            services.AddScoped<ILogRepository, LogRepository>();
            return services;
        }
    }
}
