using UniversityRegistration.Api.Models;
using UniversityRegistration.Api.Models.DTOs;

namespace UniversityRegistration.Api.Services.Interfaces
{
    public interface IEnrollmentService
    {
        Task SelectCourseAsync(int studentId, int courseId);

        Task RemoveCourseAsync(int studentId, int courseId);

        Task<List<StudentEnrollmentResponse>> GetStudentEnrollmentsAsync(int studentId);
    }
}
