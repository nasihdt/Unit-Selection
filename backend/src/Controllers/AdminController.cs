using Microsoft.AspNetCore.Mvc;
using UniversityRegistration.Api.Services.Interfaces;
using UniversityRegistration.Api.Models.Auth;

namespace UniversityRegistration.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;

        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        // POST: api/admin/login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest("درخواست نامعتبر است");

            var result = await _adminService.LoginAsync(request.Username, request.Password);

            if (result == null)
                return Unauthorized(new { message = "نام کاربری یا رمز عبور اشتباه است" });

            return Ok(result);
        }
    }
}
