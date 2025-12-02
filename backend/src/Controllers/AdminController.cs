using Microsoft.AspNetCore.Mvc;
using UniversityRegistration.Api.Services.Interfaces;

namespace UniversityRegistration.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;

        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        // GET /api/admin/login?username=admin&password=Admin@123
        [HttpGet("login")]
        public async Task<IActionResult> Login(string username, string password)
        {
            var admin = await _adminService.LoginAsync(username, password);

            if (admin == null)
                return Unauthorized(new { message = "Invalid username or password" });

            return Ok(admin);
        }
    }
}
