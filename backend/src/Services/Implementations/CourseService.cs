using UniversityRegistration.Api.Models;
using UniversityRegistration.Api.Models.DTOs;
using UniversityRegistration.Api.Repository.Interfaces;
using UniversityRegistration.Api.Services.Interfaces;

namespace UniversityRegistration.Api.Services.Implementations
{
    public class CourseService : ICourseService
    {
        private readonly ICourseRepository _repo;

        private readonly ICoursePrerequisiteRepository _prereqRepo;

        public CourseService(ICourseRepository repo, ICoursePrerequisiteRepository prereqRepo)
        {
            _repo = repo;
            _prereqRepo = prereqRepo;

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

                ExamDateTime = dto.ExamDateTime.HasValue
                ? DateTime.SpecifyKind(dto.ExamDateTime.Value, DateTimeKind.Utc)
                : null

            };

            var created = await _repo.AddAsync(course);
            return MapToResponse(created);
        }

        public async Task<bool> PatchAsync(int id, PatchCourseRequest dto)
        {
            var course = await _repo.GetByIdAsync(id);
            if (course == null)
                return false;

            if (dto.Title != null)
                course.Title = dto.Title;

            if (dto.Code != null)
                course.Code = dto.Code;

            if (dto.Units.HasValue)
                course.Units = dto.Units.Value;

            if (dto.Capacity.HasValue)
                course.Capacity = dto.Capacity.Value;

            if (dto.TeacherName != null)
                course.TeacherName = dto.TeacherName;

            if (dto.Time != null)
                course.Time = dto.Time;

            if (dto.Location != null)
                course.Location = dto.Location;

            if (dto.ExamDateTime.HasValue)
            {
                course.ExamDateTime = DateTime.SpecifyKind(
                    dto.ExamDateTime.Value,
                    DateTimeKind.Utc
                );
            }

            return await _repo.UpdateAsync(course);
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

            if (dto.ExamDateTime.HasValue)
            {
                existing.ExamDateTime = DateTime.SpecifyKind(
                    dto.ExamDateTime.Value,
                    DateTimeKind.Utc
                );
            }

            return await _repo.UpdateAsync(existing);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _repo.DeleteAsync(id);
        }

        public async Task<CourseDeleteInfoResponse?> GetDeleteInfoAsync(int courseId)
        {
            // اگر خود درس وجود نداشت
            var course = await _repo.GetByIdAsync(courseId);
            if (course == null)
                return null;

            // آیا این درس خودش پیش‌نیاز دارد؟
            var hasPrerequisites =
                await _prereqRepo.HasPrerequisitesAsync(courseId);

            // آیا این درس پیش‌نیاز درس‌های دیگر است؟
            var dependentCount =
                await _prereqRepo.DependentCoursesCountAsync(courseId);

            return new CourseDeleteInfoResponse
            {
                CourseId = courseId,
                HasPrerequisites = hasPrerequisites,
                IsPrerequisiteForOthers = dependentCount > 0,
                DependentCoursesCount = dependentCount
            };
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
                ExamDateTime = course.ExamDateTime
            };
        }

        public async Task<List<CourseResponse>> GetFilteredAsync(CourseQueryParameters q)
        {
            var courses = await _repo.GetFilteredAsync(q);
            return courses.Select(c => MapToResponse(c)).ToList();
        }
    }
}
