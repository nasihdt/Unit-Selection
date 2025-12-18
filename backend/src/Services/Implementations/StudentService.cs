using Microsoft.AspNetCore.Identity;
using UniversityRegistration.Api.Helpers;
using UniversityRegistration.Api.Models;
using UniversityRegistration.Api.Models.Auth;
using UniversityRegistration.Api.Repository.Interfaces;
using UniversityRegistration.Api.Services.Interfaces;

namespace UniversityRegistration.Api.Services.Implementations
{
    public class StudentService : IStudentService
    {
        private readonly IStudentRepository _studentRepo;
        private readonly IRefreshTokenRepository _refreshRepo;
        private readonly JwtHelper _jwtHelper;
        private readonly PasswordHasher<Student> _hasher = new();

        public StudentService(
            IStudentRepository studentRepo,
            IRefreshTokenRepository refreshRepo,
            JwtHelper jwtHelper)
        {
            _studentRepo = studentRepo;
            _refreshRepo = refreshRepo;
            _jwtHelper = jwtHelper;
        }

        public async Task<LoginResponse?> LoginAsync(string studentNumber, string password)
        {
            var student = await _studentRepo.GetByStudentNumberAsync(studentNumber);
            if (student == null) return null;

            if (_hasher.VerifyHashedPassword(student, student.Password, password)
                == PasswordVerificationResult.Failed)
                return null;

            var accessToken = _jwtHelper.GenerateToken(
                student.Id,
                student.StudentNumber,
                student.Role
            );

            var refreshToken = _jwtHelper.GenerateRefreshToken();

            await _refreshRepo.SaveAsync(new RefreshToken
            {
                Token = refreshToken,
                UserId = student.Id,
                Role = student.Role,
                ExpiresAt = DateTime.UtcNow.AddDays(7),
                IsRevoked = false
            });

            return new LoginResponse
            {
                Username = student.StudentNumber,
                Role = student.Role,
                Token = accessToken,
                RefreshToken = refreshToken
            };
        }
    }
}
