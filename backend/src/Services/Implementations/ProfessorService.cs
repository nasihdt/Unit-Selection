using Microsoft.AspNetCore.Identity;
using UniversityRegistration.Api.Helpers;
using UniversityRegistration.Api.Models;
using UniversityRegistration.Api.Models.Auth;
using UniversityRegistration.Api.Repository.Interfaces;
using UniversityRegistration.Api.Services.Interfaces;

namespace UniversityRegistration.Api.Services.Implementations
{
    public class ProfessorService : IProfessorService
    {
        private readonly IProfessorRepository _profRepo;
        private readonly IRefreshTokenRepository _refreshRepo;
        private readonly JwtHelper _jwtHelper;
        private readonly PasswordHasher<Professor> _hasher = new();

        public ProfessorService(
            IProfessorRepository profRepo,
            IRefreshTokenRepository refreshRepo,
            JwtHelper jwtHelper)
        {
            _profRepo = profRepo;
            _refreshRepo = refreshRepo;
            _jwtHelper = jwtHelper;
        }

        public async Task<LoginResponse?> LoginAsync(string professorCode, string password)
        {
            var prof = await _profRepo.GetByProfessorCodeAsync(professorCode);
            if (prof == null) return null;

            if (_hasher.VerifyHashedPassword(prof, prof.Password, password)
                == PasswordVerificationResult.Failed)
                return null;

            var accessToken = _jwtHelper.GenerateToken(
                prof.Id,
                prof.ProfessorCode,
                prof.Role
            );

            var refreshToken = _jwtHelper.GenerateRefreshToken();

            await _refreshRepo.SaveAsync(new RefreshToken
            {
                Token = refreshToken,
                UserId = prof.Id,
                Role = prof.Role,
                ExpiresAt = DateTime.UtcNow.AddDays(7),
                IsRevoked = false
            });

            return new LoginResponse
            {
                Username = prof.ProfessorCode,
                Role = prof.Role,
                Token = accessToken,
                RefreshToken = refreshToken
            };
        }
    }
}
