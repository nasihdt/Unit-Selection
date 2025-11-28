namespace UniversityRegistration.Api.Models
{
    public class Admin
    {
        public int Id { get; set; }

        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string Role { get; set; } = "Admin";
    }
}
