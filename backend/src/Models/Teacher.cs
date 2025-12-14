namespace UniversityRegistration.Api.Models
{
    public class Teacher
    {
        public int Id { get; set; }

        public string Username { get; set; } = null!;

        public string FullName { get; set; } = null!;

        public string Password { get; set; } = null!;

        public string Role { get; set; } = "Teacher";
    }
}
