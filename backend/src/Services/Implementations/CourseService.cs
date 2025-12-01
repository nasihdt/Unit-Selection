using UniversityRegistration.Api.Models;
using UniversityRegistration.Api.Repository.Interfaces;
using UniversityRegistration.Api.Services.Interfaces;

namespace UniversityRegistration.Api.Services.Implementations
{
    public class CourseService : ICourseService
    {
        private readonly ICourseRepository _repo;

        public CourseService(ICourseRepository repo)
        {
            _repo = repo;
        }

        public async Task<List<Course>> GetAllAsync()
        {
            return await _repo.GetAllAsync();
        }

        public async Task<Course?> GetByIdAsync(int id)
        {
            return await _repo.GetByIdAsync(id);
        }

        public async Task<Course> AddAsync(Course course)
        {
            return await _repo.AddAsync(course);
        }

        public async Task<bool> UpdateAsync(int id, Course course)
        {
            var existing = await _repo.GetByIdAsync(id);
            if (existing == null)
                return false;

            course.Id = id;

            return await _repo.UpdateAsync(course);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _repo.DeleteAsync(id);
        }
    }
}
