using UniversityRegistration.Api.Models;

namespace UniversityRegistration.Api.Repository.Interfaces
{
    public interface ICourseEnrollmentRepository
    {
        Task<bool> ExistsAsync(int studentId, int courseId);

        Task<int> CountByCourseAsync(int courseId);

        Task<List<CourseEnrollment>> GetByStudentAsync(int studentId);

        Task<List<CourseEnrollment>> GetByCourseAsync(int courseId);

        Task AddAsync(CourseEnrollment enrollment);

        Task RemoveAsync(CourseEnrollment enrollment);
    }
}

