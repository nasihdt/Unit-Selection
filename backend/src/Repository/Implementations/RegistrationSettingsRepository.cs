using Microsoft.EntityFrameworkCore;
using UniversityRegistration.Api.Data;
using UniversityRegistration.Api.Models;
using UniversityRegistration.Api.Repository.Interfaces;

namespace UniversityRegistration.Api.Repository.Implementations
{
    public class RegistrationSettingsRepository : IRegistrationSettingsRepository
    {
        private readonly AppDbContext _context;

        public RegistrationSettingsRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<RegistrationSettings?> GetAsync()
        {
            return await _context.RegistrationSettings
                .OrderBy(x => x.Id)
                .FirstOrDefaultAsync();
        }

        public async Task<RegistrationSettings> UpsertAsync(RegistrationSettings settings)
        {
            var existing = await GetAsync();

            if (existing == null)
            {
                _context.RegistrationSettings.Add(settings);
                await _context.SaveChangesAsync();
                return settings;
            }

            existing.MinUnits = settings.MinUnits;
            existing.MaxUnits = settings.MaxUnits;

            await _context.SaveChangesAsync();
            return existing;
        }
    }
}
