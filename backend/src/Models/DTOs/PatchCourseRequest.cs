using System.ComponentModel.DataAnnotations;

namespace UniversityRegistration.Api.Models.DTOs
{
    public class PatchCourseRequest
    {
        public string? Title { get; set; }
        public string? Code { get; set; }
        public int? Units { get; set; }
        public int? GroupNumber { get; set; }
        public int? Capacity { get; set; }
        public string? TeacherName { get; set; }
        public string? Time { get; set; }
        public string? Location { get; set; }
        public DateTime? ExamDateTime { get; set; }
    }
}
