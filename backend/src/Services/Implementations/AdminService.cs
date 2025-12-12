using Microsoft.AspNetCore.Identity;
using UniversityRegistration.Api.Helpers;
using UniversityRegistration.Api.Models;
using UniversityRegistration.Api.Models.Auth;
using UniversityRegistration.Api.Repository.Interfaces;
using UniversityRegistration.Api.Services.Interfaces;

namespace UniversityRegistration.Api.Services.Implementations
{
    public class AdminService : IAdminService
    {
        private readonly IAdminRepository _repo;
        private readonly JwtHelper _jwtHelper;
        private readonly PasswordHasher<Admin> _passwordHasher;

        public AdminService(IAdminRepository repo, JwtHelper jwtHelper)
        {
            _repo = repo;
            _jwtHelper = jwtHelper;
            _passwordHasher = new PasswordHasher<Admin>();
        }

        public async Task<LoginResponse?> LoginAsync(string username, string password)
        {
            var admin = await _repo.GetByUsernameAsync(username);

            if (admin == null)
                return null;

            var verification = _passwordHasher.VerifyHashedPassword(admin, admin.Password, password);

            if (verification == PasswordVerificationResult.Failed)
                return null;

            var token = _jwtHelper.GenerateToken(admin);

            return new LoginResponse
            {
                Username = admin.Username,
                Role = admin.Role,
                Token = token
            };
        }
    }
}
