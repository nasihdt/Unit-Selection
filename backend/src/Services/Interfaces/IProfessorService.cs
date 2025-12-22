using UniversityRegistration.Api.Models.Auth;

namespace UniversityRegistration.Api.Services.Interfaces
{
    public interface IProfessorService
    {
        Task<LoginResponse?> LoginAsync(string professorCode, string password);
    }
}
