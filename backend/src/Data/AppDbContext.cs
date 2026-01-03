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
        public DbSet<Student> Students => Set<Student>();
        public DbSet<Professor> Professors => Set<Professor>();

        public DbSet<Course> Courses => Set<Course>();
        public DbSet<CourseSession> CourseSessions => Set<CourseSession>();
        public DbSet<CoursePrerequisite> CoursePrerequisites => Set<CoursePrerequisite>();

        public DbSet<CourseEnrollment> CourseEnrollments => Set<CourseEnrollment>();

        public DbSet<RegistrationSettings> RegistrationSettings => Set<RegistrationSettings>();
        public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ===== Admin =====
            modelBuilder.Entity<Admin>(entity =>
            {
                entity.HasKey(x => x.Id);

                entity.Property(x => x.Username)
                      .IsRequired()
                      .HasMaxLength(50);

                entity.Property(x => x.Password)
                      .IsRequired()
                      .HasMaxLength(256);

                entity.Property(x => x.Role)
                      .IsRequired()
                      .HasMaxLength(20);
            });

            // ===== Student =====
            modelBuilder.Entity<Student>(entity =>
            {
                entity.HasKey(x => x.Id);

                entity.Property(x => x.StudentNumber)
                      .IsRequired()
                      .HasMaxLength(50);

                entity.Property(x => x.FullName)
                      .IsRequired()
                      .HasMaxLength(100);

                entity.Property(x => x.Password)
                      .IsRequired()
                      .HasMaxLength(256);

                entity.Property(x => x.Role)
                      .IsRequired()
                      .HasMaxLength(20);
            });

            // ===== Professor =====
            modelBuilder.Entity<Professor>(entity =>
            {
                entity.HasKey(x => x.Id);

                entity.Property(x => x.ProfessorCode)
                      .IsRequired()
                      .HasMaxLength(50);

                entity.Property(x => x.FullName)
                      .IsRequired()
                      .HasMaxLength(100);

                entity.Property(x => x.Password)
                      .IsRequired()
                      .HasMaxLength(256);

                entity.Property(x => x.Role)
                      .IsRequired()
                      .HasMaxLength(20);
            });

            // ===== Course =====
            modelBuilder.Entity<Course>(entity =>
            {
                entity.HasKey(x => x.Id);

                entity.Property(x => x.Title)
                      .IsRequired()
                      .HasMaxLength(100);

                entity.Property(x => x.Code)
                      .IsRequired()
                      .HasMaxLength(20);

                entity.Property(x => x.Units)
                      .IsRequired();

                entity.Property(x => x.GroupNumber)
                      .IsRequired();

                entity.Property(x => x.Capacity)
                      .IsRequired();

                entity.Property(x => x.TeacherName)
                      .IsRequired()
                      .HasMaxLength(100);

                entity.Ignore(x => x.Time);

                entity.Property(x => x.ExamDateTime)
                      .HasColumnType("timestamp with time zone");

                // ✅ unique Code + GroupNumber
                entity.HasIndex(x => new { x.Code, x.GroupNumber })
                      .IsUnique();
            });

            // ===== CourseSession =====
            modelBuilder.Entity<CourseSession>(entity =>
            {
                entity.HasKey(x => x.Id);

                entity.Property(x => x.Location)
                      .IsRequired()
                      .HasMaxLength(100);

                entity.Property(x => x.DayOfWeek)
                      .IsRequired();

                entity.Property(x => x.StartTime)
                      .IsRequired();

                entity.Property(x => x.EndTime)
                      .IsRequired();

                entity.HasOne(x => x.Course)
                      .WithMany(c => c.Sessions)
                      .HasForeignKey(x => x.CourseId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // ===== CoursePrerequisite =====
            modelBuilder.Entity<CoursePrerequisite>(entity =>
            {
                entity.HasKey(x => new { x.CourseId, x.PrerequisiteCourseId });

                entity.HasOne(x => x.Course)
                      .WithMany(c => c.Prerequisites)
                      .HasForeignKey(x => x.CourseId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(x => x.PrerequisiteCourse)
                      .WithMany(c => c.IsPrerequisiteFor)
                      .HasForeignKey(x => x.PrerequisiteCourseId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // ===== CourseEnrollment =====
            modelBuilder.Entity<CourseEnrollment>(entity =>
            {
                entity.HasKey(x => x.Id);

                entity.HasOne(x => x.Student)
                      .WithMany()
                      .HasForeignKey(x => x.StudentId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(x => x.Course)
                      .WithMany()
                      .HasForeignKey(x => x.CourseId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasIndex(x => new { x.StudentId, x.CourseId })
                      .IsUnique();
            });

            // ===== RegistrationSettings =====
            modelBuilder.Entity<RegistrationSettings>(entity =>
            {
                entity.HasKey(x => x.Id);

                entity.Property(x => x.MinUnits).IsRequired();
                entity.Property(x => x.MaxUnits).IsRequired();
            });

            // ===== RefreshToken =====
            modelBuilder.Entity<RefreshToken>(entity =>
            {
                entity.HasKey(x => x.Id);

                entity.Property(x => x.Token)
                      .IsRequired()
                      .HasMaxLength(500);

                entity.Property(x => x.UserId)
                      .IsRequired();

                entity.Property(x => x.Role)
                      .IsRequired()
                      .HasMaxLength(20);

                entity.Property(x => x.ExpiresAt)
                      .IsRequired();

                entity.Property(x => x.IsRevoked)
                      .IsRequired();
            });
        }
    }
}
