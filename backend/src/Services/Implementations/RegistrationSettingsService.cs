using UniversityRegistration.Api.Models;
using UniversityRegistration.Api.Repository.Interfaces;
using UniversityRegistration.Api.Services.Interfaces;

namespace UniversityRegistration.Api.Services.Implementations
{
    public class RegistrationSettingsService : IRegistrationSettingsService
    {
        private readonly IRegistrationSettingsRepository _repo;

        public RegistrationSettingsService(IRegistrationSettingsRepository repo)
        {
            _repo = repo;
        }

        public async Task<RegistrationSettings> GetOrCreateAsync()
        {
            var settings = await _repo.GetAsync();
            if (settings != null) return settings;

            // اگر رکورد وجود نداشت، یک مقدار پیش‌فرض می‌سازیم
            var created = new RegistrationSettings
            {
                MinUnits = 0,
                MaxUnits = 20
            };

            return await _repo.UpsertAsync(created);
        }

        public async Task<RegistrationSettings> UpdateAsync(int minUnits, int maxUnits)
        {
            if (minUnits < 0)
                throw new ArgumentException("MinUnits cannot be negative.");

            if (maxUnits < minUnits)
                throw new ArgumentException("MaxUnits must be >= MinUnits.");

            if (maxUnits > 24)
                throw new ArgumentException("MaxUnits cannot be more than 24.");


            var settings = await GetOrCreateAsync();

            settings.MinUnits = minUnits;
            settings.MaxUnits = maxUnits;

            return await _repo.UpsertAsync(settings);
        }
    }
}
