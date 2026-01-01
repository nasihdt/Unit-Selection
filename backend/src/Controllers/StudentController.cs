using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using UniversityRegistration.Api.Models.DTOs;
using UniversityRegistration.Api.Services.Interfaces;

namespace UniversityRegistration.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Student")]
    public class StudentController : ControllerBase
    {
        private readonly IEnrollmentService _enrollmentService;

        public StudentController(IEnrollmentService enrollmentService)
        {
            _enrollmentService = enrollmentService;
        }

        private int GetStudentId()
        {
            var studentIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrWhiteSpace(studentIdStr))
                throw new UnauthorizedAccessException("شناسه کاربر در توکن موجود نیست");

            return int.Parse(studentIdStr);
        }

        // =====================================
        // POST: api/student/select-course
        // Body: { "courseId": 12 }
        // =====================================
        [HttpPost("select-course")]
        public async Task<IActionResult> SelectCourse([FromBody] SelectCourseRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var studentId = GetStudentId();

                await _enrollmentService.SelectCourseAsync(studentId, request.CourseId);

                return Ok(new { message = "درس با موفقیت انتخاب شد" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // =====================================
        // DELETE: api/student/remove-course/{courseId}
        // =====================================
        [HttpDelete("remove-course/{courseId}")]
        public async Task<IActionResult> RemoveCourse(int courseId)
        {
            try
            {
                var studentId = GetStudentId();

                await _enrollmentService.RemoveCourseAsync(studentId, courseId);

                return Ok(new { message = "درس با موفقیت حذف شد" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // =====================================
        // GET: api/student/selected-courses
        // =====================================
        [HttpGet("selected-courses")]
        public async Task<IActionResult> GetStudentEnrollments()
        {
            var studentId = GetStudentId();

            var enrollments = await _enrollmentService.GetStudentEnrollmentsAsync(studentId);
            return Ok(enrollments);
        }
    }
}
