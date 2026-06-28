using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using StudentService.Domain.Entities;
using StudentService.Persistence.Context;

namespace StudentService.Persistence.Data
{
    public static class DatabaseSeeder
    {
        public static async Task SeedAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<StudentDbContext>();
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<StudentDbContext>>();

            logger.LogInformation("Applying Student database migrations...");
            await context.Database.MigrateAsync();
            logger.LogInformation("Student database migrations applied successfully.");

            await SeedStudentsAsync(context, logger);
        }

        private static async Task SeedStudentsAsync(StudentDbContext context, ILogger logger)
        {
            if (await context.Students.AnyAsync())
            {
                return;
            }

            logger.LogInformation("Seeding Student data...");
            var students = new List<Student>
            {
                new Student
                {
                    Studentcode = "SE171001",
                    Fullname = "Le Hoang Cuong",
                    Email = "cuonglh@fpt.edu.vn",
                    Dateofbirth = DateTime.SpecifyKind(new DateTime(2003, 5, 15), DateTimeKind.Utc),
                    Age = 23,
                    Phonenumber = "0901234001"
                },
                new Student
                {
                    Studentcode = "SE171002",
                    Fullname = "Pham Minh Duc",
                    Email = "ducpm@fpt.edu.vn",
                    Dateofbirth = DateTime.SpecifyKind(new DateTime(2003, 8, 22), DateTimeKind.Utc),
                    Age = 22,
                    Phonenumber = "0901234002"
                },
                new Student
                {
                    Studentcode = "SE171003",
                    Fullname = "Vo Thi Hoa",
                    Email = "hoavt@fpt.edu.vn",
                    Dateofbirth = DateTime.SpecifyKind(new DateTime(2004, 1, 10), DateTimeKind.Utc),
                    Age = 22,
                    Phonenumber = "0901234003"
                },
                new Student
                {
                    Studentcode = "SE171004",
                    Fullname = "Dang Quoc Khanh",
                    Email = "khanhdq@fpt.edu.vn",
                    Dateofbirth = DateTime.SpecifyKind(new DateTime(2003, 11, 3), DateTimeKind.Utc),
                    Age = 22,
                    Phonenumber = "0901234004"
                },
                new Student
                {
                    Studentcode = "SE171005",
                    Fullname = "Bui Thanh Long",
                    Email = "longbt@fpt.edu.vn",
                    Dateofbirth = DateTime.SpecifyKind(new DateTime(2003, 7, 28), DateTimeKind.Utc),
                    Age = 22,
                    Phonenumber = "0901234005"
                }
            };

            await context.Students.AddRangeAsync(students);
            await context.SaveChangesAsync();
            logger.LogInformation("Student data seeded successfully.");
        }
    }
}
