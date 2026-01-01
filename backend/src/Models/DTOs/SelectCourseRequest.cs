using System.ComponentModel.DataAnnotations;

namespace UniversityRegistration.Api.Models.DTOs
{
    public class SelectCourseRequest
    {
        [Range(1, int.MaxValue, ErrorMessage = "شناسه درس نامعتبر است")]
        public int CourseId { get; set; }
    }
}
