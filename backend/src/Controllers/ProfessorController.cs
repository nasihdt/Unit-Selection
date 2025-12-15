using Microsoft.AspNetCore.Mvc;
using UniversityRegistration.Api.Models.Auth;
using UniversityRegistration.Api.Services.Interfaces;

namespace UniversityRegistration.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProfessorController : ControllerBase
    {
        private readonly IProfessorService _professorService;

        public ProfessorController(IProfessorService professorService)
        {
            _professorService = professorService;
        }

        // POST: api/professor/login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { message = "درخواست نامعتبر است" });

            var result = await _professorService.LoginAsync(
                request.Username,
                request.Password
            );

            if (result == null)
                return Unauthorized(new { message = "کد استادی یا رمز عبور اشتباه است" });

            return Ok(result);
        }
    }
}
