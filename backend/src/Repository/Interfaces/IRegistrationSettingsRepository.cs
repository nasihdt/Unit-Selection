using UniversityRegistration.Api.Models;

namespace UniversityRegistration.Api.Repository.Interfaces
{
    public interface IRegistrationSettingsRepository
    {
        Task<RegistrationSettings?> GetAsync();
        Task<RegistrationSettings> UpsertAsync(RegistrationSettings settings);
    }
}
