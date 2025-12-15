using UniversityRegistration.Api.Models;

namespace UniversityRegistration.Api.Repository.Interfaces
{
    public interface IProfessorRepository
    {
        Task<Professor?> GetByProfessorCodeAsync(string professorCode);
    }
}
