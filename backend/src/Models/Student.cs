namespace UniversityRegistration.Api.Models
{
    public class Student
    {
        public int Id { get; set; }

        // شماره دانشجویی
        public string StudentNumber { get; set; } = null!;

        // نام کامل
        public string FullName { get; set; } = null!;

       
        public string Password { get; set; } = null!;

       
        public bool IsLabStudent { get; set; }

        public string Role { get; set; } = "Student";
    }
}
