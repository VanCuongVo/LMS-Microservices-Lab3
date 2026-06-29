using Microsoft.Extensions.DependencyInjection;
using StudentService.Application.Features;
using StudentService.Application.Interfaces;
using StudentService.Domain.IRepository;
using StudentService.Domain.IUnitOfWork;
using StudentService.Persistence.Repository;

namespace StudentService.API.Config
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDependencyInjection(this IServiceCollection services)
        {
            services.AddScoped<IStudentRepository, StudentRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IStudentService, Application.Features.StudentService>();
            services.AddGrpc();

            return services;
        }
    }
}
