using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using UniversityRegistration.Api.Models;

namespace UniversityRegistration.Api.Helpers
{
    public class JwtHelper
    {
        private readonly string _secretKey;

        public JwtHelper(string secretKey)
        {
            _secretKey = secretKey;
        }

        // =========================
        // Access Token - Admin
        // =========================
        public string GenerateToken(Admin admin)
        {
            return GenerateTokenInternal(
                userId: admin.Id,
                name: admin.Username,
                role: admin.Role
            );
        }

        // =========================
        // Access Token - Generic (Student / Professor)
        // =========================
        public string GenerateToken(int userId, string name, string role)
        {
            return GenerateTokenInternal(
                userId: userId,
                name: name,
                role: role
            );
        }

        // =========================
        // Internal Token Generator
        // =========================
        private string GenerateTokenInternal(int userId, string name, string role)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                new Claim(ClaimTypes.Name, name),
                new Claim(ClaimTypes.Role, role)
            };

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddHours(4),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        // =========================
        // Refresh Token
        // =========================
        public string GenerateRefreshToken()
        {
            var randomBytes = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomBytes);
            }

            return Convert.ToBase64String(randomBytes);
        }
    }
}
