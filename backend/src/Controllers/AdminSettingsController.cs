using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UniversityRegistration.Api.Models.DTOs;
using UniversityRegistration.Api.Services.Interfaces;

namespace UniversityRegistration.Api.Controllers
{
    [ApiController]
    [Route("api/admin/settings")]
    [Authorize(Roles = "Admin")]
    public class AdminSettingsController : ControllerBase
    {
        private readonly IRegistrationSettingsService _service;

        public AdminSettingsController(IRegistrationSettingsService service)
        {
            _service = service;
        }

        // GET: api/admin/settings/units
        [HttpGet("units")]
        public async Task<IActionResult> GetUnits()
        {
            var settings = await _service.GetOrCreateAsync();
            return Ok(settings);
        }

        // PUT: api/admin/settings/units
        [HttpPut("units")]
        public async Task<IActionResult> UpdateUnits([FromBody] UpdateUnitSettingsRequest request)
        {
            var updated = await _service.UpdateAsync(request.MinUnits, request.MaxUnits);
            return Ok(updated);
        }
    }
}
