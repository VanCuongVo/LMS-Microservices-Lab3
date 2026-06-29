using CourseService.Application.Features;
using CourseService.Application.Interfaces;
using CourseService.Domain.IRepository;
using CourseService.Domain.IUnitOfWork;
using CourseService.Infrastructure.Services;
using CourseService.Persistence.Repository;
using Microsoft.Extensions.Configuration;
using StudentService.Grpc;

namespace CourseService.API.Config
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDependencyInjection(this IServiceCollection services, IConfiguration configuration)
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

            services.AddGrpcClient<StudentGrpc.StudentGrpcClient>(o =>
            {
                o.Address = new Uri(configuration["Services:StudentServiceUrl"] ?? "https://localhost:7051");
            })
            .ConfigurePrimaryHttpMessageHandler(() =>
            {
                var handler = new HttpClientHandler();
                handler.ServerCertificateCustomValidationCallback =
                    HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
                return handler;
            });
            services.AddScoped<IStudentServiceClient, StudentGrpcClient>();

            return services;
        }
    }
}
