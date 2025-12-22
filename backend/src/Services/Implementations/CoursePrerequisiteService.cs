using UniversityRegistration.Api.Models;
using UniversityRegistration.Api.Repository.Interfaces;
using UniversityRegistration.Api.Services.Interfaces;

namespace UniversityRegistration.Api.Services.Implementations
{
    public class CoursePrerequisiteService : ICoursePrerequisiteService
    {
        private readonly ICoursePrerequisiteRepository _repo;

        public CoursePrerequisiteService(ICoursePrerequisiteRepository repo)
        {
            _repo = repo;
        }

        public async Task<List<int>> GetPrerequisiteIdsAsync(int courseId)
        {
            var exists = await _repo.CourseExistsAsync(courseId);
            if (!exists)
                throw new KeyNotFoundException("Course not found.");

            var prerequisites = await _repo.GetByCourseIdAsync(courseId);
            return prerequisites.Select(x => x.PrerequisiteCourseId).ToList();
        }

        public async Task AddAsync(int courseId, int prerequisiteCourseId)
        {
            if (courseId == prerequisiteCourseId)
                throw new InvalidOperationException("A course cannot be its own prerequisite.");

            var courseExists = await _repo.CourseExistsAsync(courseId);
            var prereqExists = await _repo.CourseExistsAsync(prerequisiteCourseId);

            if (!courseExists || !prereqExists)
                throw new KeyNotFoundException("Course or prerequisite not found.");

            var alreadyExists = await _repo.ExistsAsync(courseId, prerequisiteCourseId);
            if (alreadyExists)
                throw new InvalidOperationException("Prerequisite already exists.");

            var entity = new CoursePrerequisite
            {
                CourseId = courseId,
                PrerequisiteCourseId = prerequisiteCourseId
            };

            await _repo.AddAsync(entity);
        }

        public async Task RemoveAsync(int courseId, int prerequisiteCourseId)
        {
            var entity = await _repo.GetAsync(courseId, prerequisiteCourseId);
            if (entity == null)
                throw new KeyNotFoundException("Prerequisite not found.");

            await _repo.RemoveAsync(entity);
        }
    }
}
