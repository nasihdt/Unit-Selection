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
        public DbSet<Student> Students => Set<Student>();
        public DbSet<Professor> Professors => Set<Professor>();

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

            // ===== RefreshToken (GENERIC) =====
            modelBuilder.Entity<RefreshToken>(entity =>
            {
                entity.HasKey(rt => rt.Id);

                entity.Property(rt => rt.Token)
                      .IsRequired()
                      .HasMaxLength(500);

                entity.Property(rt => rt.UserId)
                      .IsRequired();

                entity.Property(rt => rt.Role)
                      .IsRequired()
                      .HasMaxLength(20);

                entity.Property(rt => rt.ExpiresAt)
                      .IsRequired();

                entity.Property(rt => rt.IsRevoked)
                      .IsRequired();
            });

            // ===== Student =====
            modelBuilder.Entity<Student>(entity =>
            {
                entity.HasKey(s => s.Id);

                entity.Property(s => s.StudentNumber)
                      .IsRequired()
                      .HasMaxLength(50);

                entity.Property(s => s.Password)
                      .IsRequired()
                      .HasMaxLength(256);

                entity.Property(s => s.Role)
                      .IsRequired()
                      .HasMaxLength(20);
            });

            // ===== Professor =====
            modelBuilder.Entity<Professor>(entity =>
            {
                entity.HasKey(p => p.Id);

                entity.Property(p => p.ProfessorCode)
                      .IsRequired()
                      .HasMaxLength(50);

                entity.Property(p => p.Password)
                      .IsRequired()
                      .HasMaxLength(256);

                entity.Property(p => p.Role)
                      .IsRequired()
                      .HasMaxLength(20);
            });
        }
    }
}
