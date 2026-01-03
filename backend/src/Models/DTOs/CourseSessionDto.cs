using System.ComponentModel.DataAnnotations;

namespace UniversityRegistration.Api.Models.DTOs
{
    public class CourseSessionDto
    {
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
    }
}
