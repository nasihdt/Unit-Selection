using System.ComponentModel.DataAnnotations;

namespace UniversityRegistration.Api.Models.DTOs
{
    public class CreateCourseRequest
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
        [Range(1, 500)]
        public int Capacity { get; set; }

        [Required]
        [MaxLength(100)]
        public string TeacherName { get; set; } = null!;

        [Required]
        [MaxLength(50)]
        public string Time { get; set; } = null!;

        [Required]
        [MaxLength(100)]
        public string Location { get; set; } = null!;

        [MaxLength(500)]
        public string? Description { get; set; }
    }
}
