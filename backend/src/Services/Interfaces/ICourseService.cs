using UniversityRegistration.Api.Models.DTOs;

namespace UniversityRegistration.Api.Services.Interfaces
{
    public interface ICourseService
    {
        Task<List<CourseResponse>> GetAllAsync();
        Task<CourseResponse?> GetByIdAsync(int id);
        Task<CourseResponse> AddAsync(CreateCourseRequest dto);
        Task<bool> UpdateAsync(int id, UpdateCourseRequest dto);
        Task<bool> DeleteAsync(int id);
    }
}
