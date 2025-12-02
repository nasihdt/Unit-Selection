using UniversityRegistration.Api.Models;
using UniversityRegistration.Api.Repository.Interfaces;
using UniversityRegistration.Api.Services.Interfaces;

namespace UniversityRegistration.Api.Services.Implementations
{
    public class AdminService : IAdminService
    {
        private readonly IAdminRepository _adminRepository;

        public AdminService(IAdminRepository adminRepository)
        {
            _adminRepository = adminRepository;
        }

        public async Task<Admin?> LoginAsync(string username, string password)
        {
            var admin = await _adminRepository.GetByUsernameAndPasswordAsync(username, password);
            return admin; 
        }
    }
}
