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
        private readonly IAdminRepository _adminRepo;
        private readonly IRefreshTokenRepository _refreshRepo;
        private readonly JwtHelper _jwtHelper;
        private readonly PasswordHasher<Admin> _hasher = new();

        public AdminService(
            IAdminRepository adminRepo,
            IRefreshTokenRepository refreshRepo,
            JwtHelper jwtHelper)
        {
            _adminRepo = adminRepo;
            _refreshRepo = refreshRepo;
            _jwtHelper = jwtHelper;
        }

        // =========================
        // Login
        // =========================
        public async Task<LoginResponse?> LoginAsync(string username, string password)
        {
            var admin = await _adminRepo.GetByUsernameAsync(username);
            if (admin == null) return null;

            if (_hasher.VerifyHashedPassword(admin, admin.Password, password)
                == PasswordVerificationResult.Failed)
                return null;

            var accessToken = _jwtHelper.GenerateToken(admin);
            var refreshToken = _jwtHelper.GenerateRefreshToken();

            await _refreshRepo.SaveAsync(new RefreshToken
            {
                Token = refreshToken,
                UserId = admin.Id,
                Role = admin.Role,
                ExpiresAt = DateTime.UtcNow.AddDays(7),
                IsRevoked = false
            });

            return new LoginResponse
            {
                Username = admin.Username,
                Role = admin.Role,
                Token = accessToken,
                RefreshToken = refreshToken
            };
        }

        // =========================
        // Refresh Token (Admin)
        // =========================
        public async Task<LoginResponse?> RefreshTokenAsync(string refreshToken)
        {
            var stored = await _refreshRepo.GetAsync(refreshToken);

            if (stored == null ||
                stored.IsRevoked ||
                stored.ExpiresAt < DateTime.UtcNow ||
                stored.Role != "Admin")
                return null;

            // 👇 خیلی مهم: admin رو با UserId می‌گیریم
            var admin = await _adminRepo.GetByIdAsync(stored.UserId);
            if (admin == null) return null;

            var newAccessToken = _jwtHelper.GenerateToken(admin);
            var newRefreshToken = _jwtHelper.GenerateRefreshToken();

            stored.Token = newRefreshToken;
            stored.ExpiresAt = DateTime.UtcNow.AddDays(7);
            stored.IsRevoked = false;

            await _refreshRepo.SaveAsync(stored);

            return new LoginResponse
            {
                Username = admin.Username,
                Role = admin.Role,
                Token = newAccessToken,
                RefreshToken = newRefreshToken
            };
        }
    }
}
