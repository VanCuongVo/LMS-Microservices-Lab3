using IdentityService.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace IdentityService.Persistence.Context;

public class AppIdentityDbContext :
    IdentityDbContext<
        ApplicationUser,
        ApplicationRole,
        Guid>
{
    public AppIdentityDbContext(
        DbContextOptions<AppIdentityDbContext> options)
        : base(options)
    {
    }

    public DbSet<RefreshToken> RefreshTokens { get; set; }

    public DbSet<Log> Logs { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Log>(entity =>
        {
            entity.ToTable("Logs");

            entity.HasKey(x => x.Id);

            entity.Property(x => x.Message).HasColumnType("text");
            entity.Property(x => x.Exception).HasColumnType("text");
            entity.Property(x => x.Properties).HasColumnType("jsonb");
        });
    }
}