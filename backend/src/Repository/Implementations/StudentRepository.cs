using Microsoft.EntityFrameworkCore;
using UniversityRegistration.Api.Data;
using UniversityRegistration.Api.Models;
using UniversityRegistration.Api.Repository.Interfaces;

namespace UniversityRegistration.Api.Repository.Implementations
{
    public class StudentRepository : IStudentRepository
    {
        private readonly AppDbContext _context;

        public StudentRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Student?> GetByStudentNumberAsync(string studentNumber)
        {
            return await _context.Students
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.StudentNumber == studentNumber);
        }

        public async Task<Student?> GetByIdAsync(int id)
        {
            return await _context.Students
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.Id == id);
        }


    }
}
