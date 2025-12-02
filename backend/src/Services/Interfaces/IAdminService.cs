using UniversityRegistration.Api.Models;

namespace UniversityRegistration.Api.Services.Interfaces
{
    public interface IAdminService
    {
        Task<Admin?> LoginAsync(string username, string password);
    }
}
