using Microsoft.EntityFrameworkCore;
using UniversityRegistration.Api.Data;
using UniversityRegistration.Api.Models;
using UniversityRegistration.Api.Repository.Interfaces;

namespace UniversityRegistration.Api.Repository.Implementations
{
    public class CourseEnrollmentRepository : ICourseEnrollmentRepository
    {
        private readonly AppDbContext _context;

        public CourseEnrollmentRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> ExistsAsync(int studentId, int courseId)
        {
            return await _context.CourseEnrollments
                .AnyAsync(x => x.StudentId == studentId && x.CourseId == courseId);
        }

        public async Task<int> CountByCourseAsync(int courseId)
        {
            return await _context.CourseEnrollments
                .CountAsync(x => x.CourseId == courseId);
        }

        public async Task<List<CourseEnrollment>> GetByStudentAsync(int studentId)
        {
            return await _context.CourseEnrollments
                .Include(x => x.Course)
                .Where(x => x.StudentId == studentId)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<List<CourseEnrollment>> GetByCourseAsync(int courseId)
        {
            return await _context.CourseEnrollments
                .Include(x => x.Student)
                .Where(x => x.CourseId == courseId)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task AddAsync(CourseEnrollment enrollment)
        {
            _context.CourseEnrollments.Add(enrollment);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveAsync(CourseEnrollment enrollment)
        {
            _context.CourseEnrollments.Remove(enrollment);
            await _context.SaveChangesAsync();
        }
    }
}

