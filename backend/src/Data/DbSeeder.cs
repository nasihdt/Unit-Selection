using Microsoft.AspNetCore.Identity;
using UniversityRegistration.Api.Models;

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
    }
}
