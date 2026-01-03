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

        [MaxLength(100)]
        public string? TeacherName { get; set; }

        public List<CourseSessionDto>? Sessions { get; set; }

        public DateTime? ExamDateTime { get; set; }
    }
}
