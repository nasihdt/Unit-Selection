namespace UniversityRegistration.Api.Models
{
    public class Student
    {
        public int Id { get; set; }

        public string StudentNumber { get; set; } = null!;

        public string FullName { get; set; } = null!;

        public bool IsLabStudent { get; set; }

        public string Password { get; set; } = null!;

        public string Role { get; set; } = "Student";
    }
}
