using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using CourseService.Domain.Entities;
using CourseService.Domain.Enum;
using CourseService.Persistence.Context;

namespace CourseService.Persistence.Data
{
    public static class DatabaseSeeder
    {
        public static async Task SeedAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<CourseDbContext>();
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<CourseDbContext>>();

            logger.LogInformation("Applying Course database migrations...");
            await context.Database.MigrateAsync();
            logger.LogInformation("Course database migrations applied successfully.");

            await SeedSubjectsAsync(context, logger);
            await SeedSemestersAsync(context, logger);
            await SeedCoursesAsync(context, logger);
            await SeedEnrollmentsAsync(context, logger);
        }

        private static async Task SeedSubjectsAsync(CourseDbContext context, ILogger logger)
        {
            if (await context.Subjects.AnyAsync()) return;

            logger.LogInformation("Seeding Subject data...");
            var subjects = new List<Subject>
            {
                new Subject { Subjectcode = "PRN211", Subjectname = "Basic Cross-Platform Application Programming With .NET", Credit = 3 },
                new Subject { Subjectcode = "PRN221", Subjectname = "Advanced Cross-Platform Application Programming With .NET", Credit = 3 },
                new Subject { Subjectcode = "PRN231", Subjectname = "Building Cross-Platform Back-End Application With .NET", Credit = 3 },
                new Subject { Subjectcode = "PRN232", Subjectname = "Building Cross-Platform Back-End Application With .NET (Advanced)", Credit = 3 },
                new Subject { Subjectcode = "DBI202", Subjectname = "Database Systems", Credit = 3 },
                new Subject { Subjectcode = "SWP391", Subjectname = "Application Development Project", Credit = 3 },
                new Subject { Subjectcode = "SWR302", Subjectname = "Software Requirement", Credit = 3 },
                new Subject { Subjectcode = "SWD392", Subjectname = "Software Architecture and Design", Credit = 3 }
            };

            await context.Subjects.AddRangeAsync(subjects);
            await context.SaveChangesAsync();
            logger.LogInformation("Subject data seeded successfully.");
        }

        private static async Task SeedSemestersAsync(CourseDbContext context, ILogger logger)
        {
            if (await context.Semesters.AnyAsync()) return;

            logger.LogInformation("Seeding Semester data...");
            var semesters = new List<Semester>
            {
                new Semester
                {
                    Semestername = "Spring 2025",
                    Startdate = DateTime.SpecifyKind(new DateTime(2025, 1, 6), DateTimeKind.Utc),
                    Enddate = DateTime.SpecifyKind(new DateTime(2025, 4, 30), DateTimeKind.Utc)
                },
                new Semester
                {
                    Semestername = "Summer 2025",
                    Startdate = DateTime.SpecifyKind(new DateTime(2025, 5, 12), DateTimeKind.Utc),
                    Enddate = DateTime.SpecifyKind(new DateTime(2025, 8, 31), DateTimeKind.Utc)
                },
                new Semester
                {
                    Semestername = "Fall 2025",
                    Startdate = DateTime.SpecifyKind(new DateTime(2025, 9, 8), DateTimeKind.Utc),
                    Enddate = DateTime.SpecifyKind(new DateTime(2025, 12, 20), DateTimeKind.Utc)
                },
                new Semester
                {
                    Semestername = "Spring 2026",
                    Startdate = DateTime.SpecifyKind(new DateTime(2026, 1, 5), DateTimeKind.Utc),
                    Enddate = DateTime.SpecifyKind(new DateTime(2026, 4, 28), DateTimeKind.Utc)
                },
                new Semester
                {
                    Semestername = "Summer 2026",
                    Startdate = DateTime.SpecifyKind(new DateTime(2026, 5, 11), DateTimeKind.Utc),
                    Enddate = DateTime.SpecifyKind(new DateTime(2026, 8, 30), DateTimeKind.Utc)
                }
            };

            await context.Semesters.AddRangeAsync(semesters);
            await context.SaveChangesAsync();
            logger.LogInformation("Semester data seeded successfully.");
        }

        private static async Task SeedCoursesAsync(CourseDbContext context, ILogger logger)
        {
            if (await context.Courses.AnyAsync()) return;

            logger.LogInformation("Seeding Course data...");
            var semesters = await context.Semesters.ToListAsync();
            var sp25 = semesters.First(s => s.Semestername == "Spring 2025");
            var su25 = semesters.First(s => s.Semestername == "Summer 2025");
            var fa25 = semesters.First(s => s.Semestername == "Fall 2025");
            var sp26 = semesters.First(s => s.Semestername == "Spring 2026");
            var su26 = semesters.First(s => s.Semestername == "Summer 2026");

            var courses = new List<Course>
            {
                new Course { Coursecode = "PRN231-SP25-01", Coursename = "PRN231 - Building Cross-Platform Back-End Application With .NET", Semesterid = sp25.Semesterid },
                new Course { Coursecode = "PRN232-SU25-01", Coursename = "PRN232 - Building Cross-Platform Back-End Application With .NET (Advanced)", Semesterid = su25.Semesterid },
                new Course { Coursecode = "DBI202-SP25-01", Coursename = "DBI202 - Database Systems", Semesterid = sp25.Semesterid },
                new Course { Coursecode = "SWP391-FA25-01", Coursename = "SWP391 - Application Development Project", Semesterid = fa25.Semesterid },
                new Course { Coursecode = "SWR302-SP26-01", Coursename = "SWR302 - Software Requirement", Semesterid = sp26.Semesterid },
                new Course { Coursecode = "SWD392-SU26-01", Coursename = "SWD392 - Software Architecture and Design", Semesterid = su26.Semesterid },
                new Course { Coursecode = "PRN211-SP25-01", Coursename = "PRN211 - Basic Cross-Platform Application Programming With .NET", Semesterid = sp25.Semesterid },
                new Course { Coursecode = "PRN221-FA25-01", Coursename = "PRN221 - Advanced Cross-Platform Application Programming With .NET", Semesterid = fa25.Semesterid },
                new Course { Coursecode = "PRN232-SU26-01", Coursename = "PRN232 - Building Cross-Platform Back-End Application With .NET (Advanced)", Semesterid = su26.Semesterid },
                new Course { Coursecode = "DBI202-SU25-01", Coursename = "DBI202 - Database Systems", Semesterid = su25.Semesterid }
            };

            await context.Courses.AddRangeAsync(courses);
            await context.SaveChangesAsync();
            logger.LogInformation("Course data seeded successfully.");
        }

        private static async Task SeedEnrollmentsAsync(CourseDbContext context, ILogger logger)
        {
            if (await context.Enrollments.AnyAsync()) return;

            logger.LogInformation("Seeding Enrollment data...");
            var courses = await context.Courses.ToListAsync();

            var enrollments = new List<Enrollment>
            {
                // Student 1 - Le Hoang Cuong (ID: 1)
                new Enrollment
                {
                    Studentid = 1,
                    Courseid = courses.First(c => c.Coursecode == "PRN231-SP25-01").Courseid,
                    Enrolldate = DateTime.SpecifyKind(new DateTime(2025, 1, 6), DateTimeKind.Utc),
                    Status = EnrollmentStatus.Completed
                },
                new Enrollment
                {
                    Studentid = 1,
                    Courseid = courses.First(c => c.Coursecode == "DBI202-SP25-01").Courseid,
                    Enrolldate = DateTime.SpecifyKind(new DateTime(2025, 1, 6), DateTimeKind.Utc),
                    Status = EnrollmentStatus.Completed
                },
                new Enrollment
                {
                    Studentid = 1,
                    Courseid = courses.First(c => c.Coursecode == "PRN232-SU25-01").Courseid,
                    Enrolldate = DateTime.SpecifyKind(new DateTime(2025, 5, 12), DateTimeKind.Utc),
                    Status = EnrollmentStatus.Active
                },
                // Student 2 - Pham Minh Duc (ID: 2)
                new Enrollment
                {
                    Studentid = 2,
                    Courseid = courses.First(c => c.Coursecode == "PRN211-SP25-01").Courseid,
                    Enrolldate = DateTime.SpecifyKind(new DateTime(2025, 1, 6), DateTimeKind.Utc),
                    Status = EnrollmentStatus.Completed
                },
                new Enrollment
                {
                    Studentid = 2,
                    Courseid = courses.First(c => c.Coursecode == "PRN232-SU25-01").Courseid,
                    Enrolldate = DateTime.SpecifyKind(new DateTime(2025, 5, 12), DateTimeKind.Utc),
                    Status = EnrollmentStatus.Active
                }
            };

            await context.Enrollments.AddRangeAsync(enrollments);
            await context.SaveChangesAsync();
            logger.LogInformation("Enrollment data seeded successfully.");
        }
    }
}
