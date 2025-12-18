using UniversityRegistration.Api.Models;

namespace UniversityRegistration.Api.Repository.Interfaces
{
    public interface IStudentRepository
    {
        Task<Student?> GetByStudentNumberAsync(string studentNumber);
    }
}
