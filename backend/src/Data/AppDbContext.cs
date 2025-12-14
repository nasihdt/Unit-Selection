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
        public DbSet<RegistrationSettings> RegistrationSettings => Set<RegistrationSettings>();
        public DbSet<CoursePrerequisite> CoursePrerequisites => Set<CoursePrerequisite>();

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

            // ===== RegistrationSettings =====
            modelBuilder.Entity<RegistrationSettings>(entity =>
            {
                entity.HasKey(x => x.Id);

                entity.Property(x => x.MinUnits)
                      .IsRequired();

                entity.Property(x => x.MaxUnits)
                      .IsRequired();
            });

            // ===== CoursePrerequisite (self-referencing join table) =====
            modelBuilder.Entity<CoursePrerequisite>(entity =>
            {
                // جلوگیری از تکراری شدن پیش‌نیازها
                entity.HasKey(x => new { x.CourseId, x.PrerequisiteCourseId });

                entity.HasOne(x => x.Course)
                      .WithMany(c => c.Prerequisites)
                      .HasForeignKey(x => x.CourseId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(x => x.PrerequisiteCourse)
                      .WithMany(c => c.IsPrerequisiteFor)
                      .HasForeignKey(x => x.PrerequisiteCourseId)
                      .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}
