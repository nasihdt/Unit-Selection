namespace UniversityRegistration.Api.Models
{
    public class Student
    {
        public int Id { get; set; }

        public string StudentNumber { get; set; } = null!;

        public string Password { get; set; } = null!;

        public string Role { get; set; } = "Student";
    }
}
