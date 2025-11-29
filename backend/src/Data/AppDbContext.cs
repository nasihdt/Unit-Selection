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

                // Seed یک ادمین اولیه
                entity.HasData(new Admin
                {
                    Id = 1,
                    Username = "admin",
                    Password = "Admin@123", // برای پروژه دانشگاهی ساده نگه‌داشتیم
                    Role = "Admin"
                });
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

                // چند تا Course نمونه (Seed)
                modelBuilder.Entity<Course>().HasData(
                    new Course
                    {
                        Id = 1,
                        Title = "برنامه‌نویسی ۱",
                        Code = "CS101",
                        Units = 3,
                        Capacity = 40,
                        TeacherName = "دکتر احمدی",
                        Time = "شنبه 10-12",
                        Location = "کلاس 201",
                        Description = "درس مقدماتی برنامه‌نویسی"
                    },
                    new Course
                    {
                        Id = 2,
                        Title = "پایگاه داده",
                        Code = "CS202",
                        Units = 3,
                        Capacity = 35,
                        TeacherName = "دکتر رضایی",
                        Time = "دوشنبه 8-10",
                        Location = "کلاس 105",
                        Description = "مبانی سیستم‌های مدیریت پایگاه داده"
                    }
                );
            });
        }
    }
}
