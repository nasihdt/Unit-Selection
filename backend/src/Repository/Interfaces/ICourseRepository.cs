using UniversityRegistration.Api.Models;
using UniversityRegistration.Api.Models.DTOs;

namespace UniversityRegistration.Api.Repository.Interfaces
{
    public interface ICourseRepository
    {
        Task<List<Course>> GetAllAsync();
        Task<Course?> GetByIdAsync(int id);
        Task<Course?> FindByCodeAsync(string code);
        Task<Course> AddAsync(Course course);
        Task<bool> UpdateAsync(Course course);
        Task<bool> DeleteAsync(int id);

        Task<List<Course>> GetFilteredAsync(CourseQueryParameters q);
    }
}
