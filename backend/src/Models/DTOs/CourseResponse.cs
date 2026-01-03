using System.ComponentModel.DataAnnotations;

namespace UniversityRegistration.Api.Models.DTOs
{
    public class CourseResponse
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string Code { get; set; } = null!;
        public int Units { get; set; }
        public int GroupNumber { get; set; }
        public int Capacity { get; set; }
        public string TeacherName { get; set; } = null!;
        public string Time { get; set; } = null!;
        public int DayOfWeek { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public string Location { get; set; } = null!;
        public DateTime? ExamDateTime { get; set; }
    }
}
