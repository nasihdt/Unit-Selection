using Microsoft.EntityFrameworkCore;
using UniversityRegistration.Api.Data;
using UniversityRegistration.Api.Models;
using UniversityRegistration.Api.Models.DTOs;
using UniversityRegistration.Api.Repository.Interfaces;

namespace UniversityRegistration.Api.Repository.Implementations
{
    public class CourseRepository : ICourseRepository
    {
        private readonly AppDbContext _context;

        public CourseRepository(AppDbContext context)
        {
            _context = context;
        }

        // ==========================
        // Get All (با Sessions)
        // ==========================
        public async Task<List<Course>> GetAllWithSessionsAsync()
        {
            return await _context.Courses
                .Include(c => c.Sessions)
                .AsNoTracking()
                .ToListAsync();
        }

        // ==========================
        // Get By Id (با Sessions)
        // ==========================
        public async Task<Course?> GetByIdWithSessionsAsync(int id)
        {
            return await _context.Courses
                .Include(c => c.Sessions)
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        // ==========================
        // Find By Code
        // ==========================
        public async Task<Course?> FindByCodeAsync(string code)
        {
            var clean = code.Trim();

            return await _context.Courses
                .Include(c => c.Sessions)
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Code == clean);
        }

        // ==========================
        // Add
        // ==========================
        public async Task<Course> AddAsync(Course course)
        {
            _context.Courses.Add(course);
            await _context.SaveChangesAsync();
            return course;
        }

        // ==========================
        // Update
        // ==========================
        public async Task<bool> UpdateAsync(Course course)
        {
            // اینجا لازم داریم Sessions هم لود شده باشه
            var existing = await _context.Courses
                .Include(c => c.Sessions)
                .FirstOrDefaultAsync(c => c.Id == course.Id);

            if (existing == null)
                return false;

            existing.Title = course.Title;
            existing.Code = course.Code;
            existing.Units = course.Units;
            existing.GroupNumber = course.GroupNumber;
            existing.Capacity = course.Capacity;
            existing.TeacherName = course.TeacherName;

            // نمایش
            existing.Time = course.Time;

            existing.ExamDateTime = course.ExamDateTime;

            // ✅ جایگزینی کامل Sessions
            existing.Sessions.Clear();
            foreach (var s in course.Sessions)
            {
                existing.Sessions.Add(new CourseSession
                {
                    DayOfWeek = s.DayOfWeek,
                    StartTime = s.StartTime,
                    EndTime = s.EndTime,
                    Location = s.Location
                });
            }

            await _context.SaveChangesAsync();
            return true;
        }

        // ==========================
        // Delete
        // ==========================
        public async Task<bool> DeleteAsync(int id)
        {
            var course = await _context.Courses.FindAsync(id);
            if (course == null)
                return false;

            _context.Courses.Remove(course);
            await _context.SaveChangesAsync();
            return true;
        }

        // ==========================
        // Filter
        // ==========================
        public async Task<List<Course>> GetFilteredAsync(CourseQueryParameters q)
        {
            IQueryable<Course> query = _context.Courses
                .Include(c => c.Sessions)
                .AsNoTracking();

            if (!string.IsNullOrWhiteSpace(q.Search))
            {
                var s = q.Search.Trim();
                query = query.Where(c =>
                    c.Title.Contains(s) ||
                    c.Code.Contains(s) ||
                    c.TeacherName.Contains(s));
            }

            if (!string.IsNullOrWhiteSpace(q.Code))
            {
                var code = q.Code.Trim();
                query = query.Where(c => c.Code.Contains(code));
            }

            if (!string.IsNullOrWhiteSpace(q.TeacherName))
            {
                var teacher = q.TeacherName.Trim();
                query = query.Where(c => c.TeacherName.Contains(teacher));
            }

            if (q.Units.HasValue)
                query = query.Where(c => c.Units == q.Units.Value);

            if (q.MinCapacity.HasValue)
                query = query.Where(c => c.Capacity >= q.MinCapacity.Value);

            if (q.OnlyAvailable == true)
                query = query.Where(c => c.Capacity > 0);

            return await query.ToListAsync();
        }

        // ==========================
        //  برای چک تداخل مکانی
        // ==========================
        public async Task<List<Course>> GetCoursesByLocationsAsync(List<string> locations)
        {
            var cleaned = locations
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Select(x => x.Trim())
                .Distinct()
                .ToList();

            return await _context.Courses
                .Include(c => c.Sessions)
                .AsNoTracking()
                .Where(c => c.Sessions.Any(s => cleaned.Contains(s.Location)))
                .ToListAsync();
        }

        // ==========================
        //  برای چک تکراری بودن Code + Group
        // ==========================
        public async Task<bool> ExistsByCodeAndGroupAsync(string code, int groupNumber, int? excludeCourseId = null)
        {
            var cleanCode = code.Trim();

            IQueryable<Course> query = _context.Courses.AsNoTracking()
                .Where(c => c.Code == cleanCode && c.GroupNumber == groupNumber);

            if (excludeCourseId.HasValue)
                query = query.Where(c => c.Id != excludeCourseId.Value);

            return await query.AnyAsync();
        }
    }
}
