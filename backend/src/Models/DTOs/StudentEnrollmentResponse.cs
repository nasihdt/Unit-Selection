namespace UniversityRegistration.Api.Models.DTOs
{
    public class StudentEnrollmentResponse
    {
        public int EnrollmentId { get; set; }
        public int CourseId { get; set; }

        public CourseResponse Course { get; set; } = null!;
    }
}
