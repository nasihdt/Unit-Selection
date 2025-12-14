using Microsoft.AspNetCore.Identity;
using UniversityRegistration.Api.Helpers;
using UniversityRegistration.Api.Models;
using UniversityRegistration.Api.Models.Auth;
using UniversityRegistration.Api.Repository.Interfaces;
using UniversityRegistration.Api.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

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

        // Login با username و password
        public async Task<LoginResponse?> LoginAsync(string username, string password)
        {
            var admin = await _repo.GetByUsernameAsync(username);

            if (admin == null)
                return null;

            var verification = _passwordHasher.VerifyHashedPassword(admin, admin.Password, password);

            if (verification == PasswordVerificationResult.Failed)
                return null;

            // ساخت Access Token و Refresh Token
            var accessToken = _jwtHelper.GenerateToken(admin);
            var refreshToken = _jwtHelper.GenerateRefreshToken();

            // ذخیره Refresh Token در دیتابیس
            var tokenEntity = new RefreshToken
            {
                Token = refreshToken,
                AdminId = admin.Id,
                ExpiresAt = DateTime.UtcNow.AddDays(7),
                IsRevoked = false
            };
            await _repo.SaveRefreshTokenAsync(tokenEntity);

            return new LoginResponse
            {
                Username = admin.Username,
                Role = admin.Role,
                Token = accessToken,
                RefreshToken = refreshToken
            };
        }

        // Refresh کردن Access Token با استفاده از Refresh Token
        public async Task<LoginResponse?> RefreshTokenAsync(string refreshToken)
        {
            var storedToken = await _repo.GetRefreshTokenAsync(refreshToken);

            if (storedToken == null || storedToken.IsRevoked || storedToken.ExpiresAt < DateTime.UtcNow)
                return null;

            var admin = await _repo.GetByUsernameAsync(storedToken.Admin.Username);

            if (admin == null)
                return null;

            // ساخت Access Token جدید
            var newAccessToken = _jwtHelper.GenerateToken(admin);

            // (اختیاری) تولید Refresh Token جدید و بروزرسانی
            var newRefreshToken = _jwtHelper.GenerateRefreshToken();
            storedToken.Token = newRefreshToken;
            storedToken.ExpiresAt = DateTime.UtcNow.AddDays(7);
            storedToken.IsRevoked = false;

            await _repo.SaveRefreshTokenAsync(storedToken);

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
