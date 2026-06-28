using Microsoft.EntityFrameworkCore;
using CourseService.Domain.Entities;

namespace CourseService.Persistence.Context
{
    public class CourseDbContext : DbContext
    {
        public CourseDbContext(DbContextOptions<CourseDbContext> options)
            : base(options)
        {
        }

        public DbSet<Course> Courses { get; set; }
        public DbSet<Semester> Semesters { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Semester>(entity =>
            {
                entity.ToTable("Semesters");
                entity.HasKey(e => e.Semesterid);
                entity.Property(e => e.Semesterid).ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<Course>(entity =>
            {
                entity.ToTable("Courses");
                entity.HasKey(e => e.Courseid);
                entity.Property(e => e.Courseid).ValueGeneratedOnAdd();
                entity.HasOne(d => d.Semester)
                    .WithMany(p => p.Courses)
                    .HasForeignKey(d => d.Semesterid)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Enrollment>(entity =>
            {
                entity.ToTable("Enrollments");
                entity.HasKey(e => e.Enrollmentid);
                entity.Property(e => e.Enrollmentid).ValueGeneratedOnAdd();
                entity.HasOne(d => d.Course)
                    .WithMany(p => p.Enrollments)
                    .HasForeignKey(d => d.Courseid)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Subject>(entity =>
            {
                entity.ToTable("Subjects");
                entity.HasKey(e => e.Subjectid);
                entity.Property(e => e.Subjectid).ValueGeneratedOnAdd();
            });
        }
    }
}
