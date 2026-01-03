using UniversityRegistration.Api.Models;
using UniversityRegistration.Api.Models.DTOs;

namespace UniversityRegistration.Api.Repository.Interfaces
{
    public interface ICourseRepository
    {
        Task<List<Course>> GetAllWithSessionsAsync();
        Task<Course?> GetByIdWithSessionsAsync(int id);

        Task<Course?> FindByCodeAsync(string code);

        Task<Course> AddAsync(Course course);
        Task<bool> UpdateAsync(Course course);
        Task<bool> DeleteAsync(int id);

        Task<List<Course>> GetFilteredAsync(CourseQueryParameters q);

        // برای چک تداخل مکانی (همه درس‌هایی که در لوکیشن‌های داده شده جلسه دارند)
        Task<List<Course>> GetCoursesByLocationsAsync(List<string> locations);

        // برای چک تکراری بودن Code + Group
        Task<bool> ExistsByCodeAndGroupAsync(string code, int groupNumber, int? excludeCourseId = null);
    }
}
