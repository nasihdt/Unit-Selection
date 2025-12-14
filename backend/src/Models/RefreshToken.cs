namespace UniversityRegistration.Api.Models
{
    public class RefreshToken
    {
        public int Id { get; set; }

        public string Token { get; set; } = null!;

        public DateTime ExpiresAt { get; set; }

        public bool IsRevoked { get; set; }

        // ===== Relation =====
        public int AdminId { get; set; }
        public Admin Admin { get; set; } = null!;
    }
}
