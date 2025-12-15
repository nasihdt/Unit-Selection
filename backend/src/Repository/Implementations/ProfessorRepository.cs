using Microsoft.EntityFrameworkCore;
using UniversityRegistration.Api.Data;
using UniversityRegistration.Api.Models;
using UniversityRegistration.Api.Repository.Interfaces;

namespace UniversityRegistration.Api.Repository.Implementations
{
    public class ProfessorRepository : IProfessorRepository
    {
        private readonly AppDbContext _context;

        public ProfessorRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Professor?> GetByProfessorCodeAsync(string professorCode)
        {
            return await _context.Professors
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.ProfessorCode == professorCode);
        }
    }
}
