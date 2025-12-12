using UniversityRegistration.Api.Models;
using UniversityRegistration.Api.Models.DTOs;
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

        public async Task<List<CourseResponse>> GetAllAsync()
        {
            var courses = await _repo.GetAllAsync();
            return courses.Select(c => MapToResponse(c)).ToList();
        }

        public async Task<CourseResponse?> GetByIdAsync(int id)
        {
            var course = await _repo.GetByIdAsync(id);
            if (course == null)
                return null;

            return MapToResponse(course);
        }

        public async Task<CourseResponse> AddAsync(CreateCourseRequest dto)
        {
            var course = new Course
            {
                Title = dto.Title,
                Code = dto.Code,
                Units = dto.Units,
                Capacity = dto.Capacity,
                TeacherName = dto.TeacherName,
                Time = dto.Time,
                Location = dto.Location,
                Description = dto.Description
            };

            var created = await _repo.AddAsync(course);
            return MapToResponse(created);
        }

        public async Task<bool> UpdateAsync(int id, UpdateCourseRequest dto)
        {
            var existing = await _repo.GetByIdAsync(id);
            if (existing == null)
                return false;

            existing.Title = dto.Title;
            existing.Code = dto.Code;
            existing.Units = dto.Units;
            existing.Capacity = dto.Capacity;
            existing.TeacherName = dto.TeacherName;
            existing.Time = dto.Time;
            existing.Location = dto.Location;
            existing.Description = dto.Description;

            return await _repo.UpdateAsync(existing);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _repo.DeleteAsync(id);
        }

        // ==========================
        // Mapping Helper
        // ==========================
        private CourseResponse MapToResponse(Course course)
        {
            return new CourseResponse
            {
                Id = course.Id,
                Title = course.Title,
                Code = course.Code,
                Units = course.Units,
                Capacity = course.Capacity,
                TeacherName = course.TeacherName,
                Time = course.Time,
                Location = course.Location,
                Description = course.Description
            };
        }
    }
}
