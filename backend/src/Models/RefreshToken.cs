namespace UniversityRegistration.Api.Models
{
    public class RefreshToken
    {
        public int Id { get; set; }

        public string Token { get; set; } = null!;

        public DateTime ExpiresAt { get; set; }

        public bool IsRevoked { get; set; }

        // ===== Generic User Info =====
        public int UserId { get; set; }
        public string Role { get; set; } = null!;
    }
}
