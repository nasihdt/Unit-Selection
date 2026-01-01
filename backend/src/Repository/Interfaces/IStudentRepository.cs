using UniversityRegistration.Api.Models;

namespace UniversityRegistration.Api.Repository.Interfaces
{
    public interface IStudentRepository
    {
        Task<Student?> GetByIdAsync(int id);
        Task<Student?> GetByStudentNumberAsync(string studentNumber);
    }
}
