using UniversityRegistration.Api.Models;
using UniversityRegistration.Api.Models.Auth;

namespace UniversityRegistration.Api.Services.Interfaces
{
    public interface IAdminService
    {
        Task<LoginResponse?> LoginAsync(string username, string password);
    }
}
