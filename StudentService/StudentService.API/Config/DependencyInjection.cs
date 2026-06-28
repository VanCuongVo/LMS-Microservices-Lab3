using Microsoft.Extensions.DependencyInjection;
using StudentService.Application.Features;
using StudentService.Application.Interfaces;
using StudentService.Domain.IRepository;
using StudentService.Domain.IUnitOfWork;
using StudentService.Infrastructure.Services;
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

            // Register HttpClient for CourseService cross-service calls
            services.AddHttpClient<ICourseServiceClient, CourseServiceClient>();

            return services;
        }
    }
}
