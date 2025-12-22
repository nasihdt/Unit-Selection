using Microsoft.AspNetCore.Mvc;
using UniversityRegistration.Api.Models.Auth;
using UniversityRegistration.Api.Services.Interfaces;

namespace UniversityRegistration.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentController : ControllerBase
    {
        private readonly IStudentService _studentService;

        public StudentController(IStudentService studentService)
        {
            _studentService = studentService;
        }

        // POST: api/student/login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { message = "درخواست نامعتبر است" });

            var result = await _studentService.LoginAsync(
                request.Username,
                request.Password
            );

            if (result == null)
                return Unauthorized(new { message = "شماره دانشجویی یا رمز عبور اشتباه است" });

            return Ok(result);
        }
    }
}
