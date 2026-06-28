using Microsoft.Extensions.DependencyInjection;
using CourseService.Application.Features;
using CourseService.Application.Interfaces;
using CourseService.Domain.IRepository;
using CourseService.Domain.IUnitOfWork;
using CourseService.Infrastructure.Services;
using CourseService.Persistence.Repository;

namespace CourseService.API.Config
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDependencyInjection(this IServiceCollection services)
        {
            // Register repositories
            services.AddScoped<ICourseRepository, CourseRepository>();
            services.AddScoped<ISemesterRepository, SemesterRepository>();
            services.AddScoped<ISubjectRepository, SubjectRepository>();
            services.AddScoped<IEnrollmentRepository, EnrollmentRepository>();

            // Register Unit of Work
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            // Register services
            services.AddScoped<ICourseService, Application.Features.CourseService>();
            services.AddScoped<IEnrollmentService, EnrollmentService>();
            services.AddScoped<ISemestersService, SemestersService>();
            services.AddScoped<ISubjectService, SubjectService>();

            // Register HttpClient for StudentService cross-service calls
            services.AddHttpClient<IStudentServiceClient, StudentServiceClient>();

            return services;
        }
    }
}
