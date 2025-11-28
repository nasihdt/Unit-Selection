namespace UniversityRegistration.Api.Models
{
    public class Course
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string Code { get; set; } = null!;
        public int Units { get; set; }
        public int Capacity { get; set; }
        public string TeacherName { get; set; } = null!;
        public string Time { get; set; } = null!;
        public string Location { get; set; } = null!;
    }
}
