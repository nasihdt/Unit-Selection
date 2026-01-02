using Microsoft.EntityFrameworkCore;
using UniversityRegistration.Api.Data;
using UniversityRegistration.Api.Models;
using UniversityRegistration.Api.Repository.Interfaces;

namespace UniversityRegistration.Api.Repository.Implementations
{
    public class CoursePrerequisiteRepository : ICoursePrerequisiteRepository
    {
        private readonly AppDbContext _context;

        public CoursePrerequisiteRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> CourseExistsAsync(int courseId)
        {
            return await _context.Courses.AnyAsync(c => c.Id == courseId);
        }

        public async Task<bool> ExistsAsync(int courseId, int prerequisiteCourseId)
        {
            return await _context.CoursePrerequisites.AnyAsync(x =>
                x.CourseId == courseId &&
                x.PrerequisiteCourseId == prerequisiteCourseId);
        }

        public async Task<List<CoursePrerequisite>> GetByCourseIdAsync(int courseId)
        {
            return await _context.CoursePrerequisites
                .Where(x => x.CourseId == courseId)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<List<int>> GetPrerequisiteIdsAsync(int courseId)
        {
            return await _context.CoursePrerequisites
                .Where(x => x.CourseId == courseId)
                .Select(x => x.PrerequisiteCourseId)
                .ToListAsync();
        }

        public async Task<CoursePrerequisite?> GetAsync(int courseId, int prerequisiteCourseId)
        {
            return await _context.CoursePrerequisites
                .FirstOrDefaultAsync(x =>
                    x.CourseId == courseId &&
                    x.PrerequisiteCourseId == prerequisiteCourseId);
        }

        public async Task AddAsync(CoursePrerequisite entity)
        {
            _context.CoursePrerequisites.Add(entity);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveAsync(CoursePrerequisite entity)
        {
            _context.CoursePrerequisites.Remove(entity);
            await _context.SaveChangesAsync();
        }

        // آیا این درس خودش پیش‌نیاز دارد؟
        public async Task<bool> HasPrerequisitesAsync(int courseId)
        {
            return await _context.CoursePrerequisites
                .AnyAsync(x => x.CourseId == courseId);
        }

        // این درس پیش‌نیاز چند درس دیگر است؟
        public async Task<int> DependentCoursesCountAsync(int courseId)
        {
            return await _context.CoursePrerequisites
                .CountAsync(x => x.PrerequisiteCourseId == courseId);
        }
    }
}
