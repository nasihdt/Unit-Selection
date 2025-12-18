using UniversityRegistration.Api.Models.Auth;

namespace UniversityRegistration.Api.Services.Interfaces
{
    public interface IStudentService
    {
        Task<LoginResponse?> LoginAsync(string studentNumber, string password);
    }
}
