using UniversityRegistration.Api.Models;

namespace UniversityRegistration.Api.Services.Interfaces
{
    public interface IEnrollmentService
    {
        Task SelectCourseAsync(int studentId, int courseId);

        Task RemoveCourseAsync(int studentId, int courseId);

        Task<List<CourseEnrollment>> GetStudentEnrollmentsAsync(int studentId);
    }
}
