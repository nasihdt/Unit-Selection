using System.ComponentModel.DataAnnotations;

namespace UniversityRegistration.Api.Models.DTOs
{
    public class PatchCourseRequest
    {
        public string? Title { get; set; }
        public string? Code { get; set; }

        [Range(1, 5)]
        public int? Units { get; set; }

        [Range(1, 50)]
        public int? GroupNumber { get; set; }

        [Range(1, 500)]
        public int? Capacity { get; set; }

        public string? TeacherName { get; set; }

        [Range(0, 6, ErrorMessage = "روز هفته نامعتبر است (0 تا 6).")]
        public int? DayOfWeek { get; set; }

        public TimeSpan? StartTime { get; set; }
        public TimeSpan? EndTime { get; set; }

        [MaxLength(100)]
        public string? Location { get; set; }

        public DateTime? ExamDateTime { get; set; }
    }
}
