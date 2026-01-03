using System.ComponentModel.DataAnnotations;

namespace UniversityRegistration.Api.Models.DTOs
{
    public class UpdateCourseRequest
    {
        [Required]
        [MaxLength(100)]
        public string Title { get; set; } = null!;

        [Required]
        [MaxLength(20)]
        public string Code { get; set; } = null!;

        [Required]
        [Range(1, 5)]
        public int Units { get; set; }

        [Required]
        [Range(1, 50)]
        public int GroupNumber { get; set; }

        [Required]
        [Range(1, 500)]
        public int Capacity { get; set; }

        [Required]
        [MaxLength(100)]
        public string TeacherName { get; set; } = null!;

        [Required]
        [Range(0, 6, ErrorMessage = "روز هفته نامعتبر است (0 تا 6).")]
        public int DayOfWeek { get; set; }

        [Required]
        public TimeSpan StartTime { get; set; }

        [Required]
        public TimeSpan EndTime { get; set; }

        [Required]
        [MaxLength(100)]
        public string Location { get; set; } = null!;

        public DateTime? ExamDateTime { get; set; }
    }
}
