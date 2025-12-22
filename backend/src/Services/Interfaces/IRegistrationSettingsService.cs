using UniversityRegistration.Api.Models;

namespace UniversityRegistration.Api.Services.Interfaces
{
    public interface IRegistrationSettingsService
    {
        Task<RegistrationSettings> GetOrCreateAsync();
        Task<RegistrationSettings> UpdateAsync(int minUnits, int maxUnits);
    }
}
