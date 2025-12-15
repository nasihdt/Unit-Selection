using UniversityRegistration.Api.Models;

namespace UniversityRegistration.Api.Repository.Interfaces
{
    public interface ICoursePrerequisiteRepository
    {
        Task<bool> CourseExistsAsync(int courseId);

        Task<bool> ExistsAsync(int courseId, int prerequisiteCourseId);

        Task<List<CoursePrerequisite>> GetByCourseIdAsync(int courseId);

        Task<CoursePrerequisite?> GetAsync(int courseId, int prerequisiteCourseId);

        Task AddAsync(CoursePrerequisite entity);

        Task RemoveAsync(CoursePrerequisite entity);
    }
}
