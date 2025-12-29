using Microsoft.AspNetCore.Mvc;
using UniversityRegistration.Api.Models.DTOs;
using UniversityRegistration.Api.Services.Interfaces;

namespace UniversityRegistration.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentController : ControllerBase
    {
        private readonly IEnrollmentService _enrollmentService;

        public StudentController(IEnrollmentService enrollmentService)
        {
            _enrollmentService = enrollmentService;
        }

        // =====================================
        // POST: api/student/select-course
        // =====================================
        [HttpPost("select-course")]
        public async Task<IActionResult> SelectCourse([FromBody] SelectCourseRequest request)
        {
            try
            {
                await _enrollmentService.SelectCourseAsync(request.StudentId, request.CourseId);
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
        public async Task<IActionResult> RemoveCourse(int courseId, [FromQuery] int studentId)
        {
            try
            {
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
        public async Task<IActionResult> GetStudentEnrollments([FromQuery] int studentId)
        {
            var enrollments = await _enrollmentService.GetStudentEnrollmentsAsync(studentId);
            return Ok(enrollments);
        }
    }
}
