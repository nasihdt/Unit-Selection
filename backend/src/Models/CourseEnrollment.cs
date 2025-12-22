namespace UniversityRegistration.Api.Models
{
    public class CourseEnrollment
    {
        public int Id { get; set; }

        // ===== Relations =====
        public int StudentId { get; set; }
        public Student Student { get; set; } = null!;

        public int CourseId { get; set; }
        public Course Course { get; set; } = null!;

        // ===== Metadata =====
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
