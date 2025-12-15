using Microsoft.AspNetCore.Identity;
using UniversityRegistration.Api.Models;
using System.Linq;


namespace UniversityRegistration.Api.Data
{
    public static class DbSeeder
    {
        public static void SeedAdmins(AppDbContext context)
        {
            if (!context.Admins.Any())
            {
                var admin = new Admin
                {
                    Username = "admin",
                    Role = "Admin"
                };

                var hasher = new PasswordHasher<Admin>();
                admin.Password = hasher.HashPassword(admin, "Admin@123");

                context.Admins.Add(admin);
                context.SaveChanges();
            }
        }

        public static void SeedStudents(AppDbContext context)
        {
            if (!context.Students.Any())
            {
                var s = new Student
                {
                    StudentNumber = "40123456",
                    FullName = "Test Student",
                    IsLabStudent = false,
                    Role = "Student"
                };

                var hasher = new PasswordHasher<Student>();
                s.Password = hasher.HashPassword(s, "Student@123");
                context.Students.Add(s);
                context.SaveChanges();
            }
        }

        public static void SeedProfessors(AppDbContext context)
        {
            if (!context.Professors.Any())
            {
                var p = new Professor
                {
                    ProfessorCode = "P1001",
                    FullName = "Test Professor",
                    Role = "Professor"
                };


                var hasher = new PasswordHasher<Professor>();
                p.Password = hasher.HashPassword(p, "Professor@123");
                context.Professors.Add(p);
                context.SaveChanges();
            }
        }

    }
}
