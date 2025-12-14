using Microsoft.EntityFrameworkCore;
using UniversityRegistration.Api.Models;

namespace UniversityRegistration.Api.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Admin> Admins => Set<Admin>();
        public DbSet<Course> Courses => Set<Course>();
        public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ===== Admin =====
            modelBuilder.Entity<Admin>(entity =>
            {
                entity.HasKey(a => a.Id);

                entity.Property(a => a.Username)
                      .IsRequired()
                      .HasMaxLength(50);

                entity.Property(a => a.Password)
                      .IsRequired()
                      .HasMaxLength(256);

                entity.Property(a => a.Role)
                      .IsRequired()
                      .HasMaxLength(20);
            });

            // ===== Course =====
            modelBuilder.Entity<Course>(entity =>
            {
                entity.HasKey(c => c.Id);

                entity.Property(c => c.Title)
                      .IsRequired()
                      .HasMaxLength(100);

                entity.Property(c => c.Code)
                      .IsRequired()
                      .HasMaxLength(20);

                entity.Property(c => c.Units)
                      .IsRequired();

                entity.Property(c => c.Capacity)
                      .IsRequired();

                entity.Property(c => c.TeacherName)
                      .IsRequired()
                      .HasMaxLength(100);

                entity.Property(c => c.Time)
                      .IsRequired()
                      .HasMaxLength(50);

                entity.Property(c => c.Location)
                      .IsRequired()
                      .HasMaxLength(100);

                entity.Property(c => c.Description)
                      .HasMaxLength(500);
            });

            // ===== RefreshToken =====
            modelBuilder.Entity<RefreshToken>(entity =>
            {
                entity.HasKey(rt => rt.Id);

                entity.Property(rt => rt.Token)
                      .IsRequired()
                      .HasMaxLength(500);

                entity.Property(rt => rt.ExpiresAt)
                      .IsRequired();

                entity.Property(rt => rt.IsRevoked)
                      .IsRequired();

                entity.HasOne(rt => rt.Admin)
                      .WithMany()
                      .HasForeignKey(rt => rt.AdminId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

        }
    }
}
