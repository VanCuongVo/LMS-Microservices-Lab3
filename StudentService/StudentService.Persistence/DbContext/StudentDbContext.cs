using Microsoft.EntityFrameworkCore;
using StudentService.Domain.Entities;

namespace StudentService.Persistence.Context
{
    public class StudentDbContext : DbContext
    {
        public StudentDbContext(DbContextOptions<StudentDbContext> options)
            : base(options)
        {
        }

        public DbSet<Student> Students { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Student>(entity =>
            {
                entity.ToTable("Students");
                entity.HasKey(e => e.Studentid);
                entity.Property(e => e.Studentid).ValueGeneratedOnAdd();
            });
        }
    }
}
