using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UniversityRegistration.Api.Data;
using UniversityRegistration.Api.Models.DTOs;
using UniversityRegistration.Api.Services.Interfaces;

namespace UniversityRegistration.Api.Controllers
{
    [ApiController]
    [Route("api/admin/courses/{courseId:int}/prerequisites")]
    [Authorize(Roles = "Admin")]
    public class AdminCoursePrerequisitesController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ICoursePrerequisiteService _service;

        public AdminCoursePrerequisitesController(
            AppDbContext context,
            ICoursePrerequisiteService service)
        {
            _context = context;
            _service = service;
        }

        // =====================================
        // GET: api/admin/courses/{courseId}/prerequisites
        // =====================================
        [HttpGet]
        public async Task<IActionResult> GetPrerequisites(int courseId)
        {
            var exists = await _context.Courses.AnyAsync(c => c.Id == courseId);
            if (!exists)
                return NotFound(new { message = "درس موردنظر یافت نشد." });

            var prerequisites = await _context.CoursePrerequisites
                .Where(x => x.CourseId == courseId)
                .Select(x => new CourseResponse
                {
                    Id = x.PrerequisiteCourse.Id,
                    Title = x.PrerequisiteCourse.Title,
                    Code = x.PrerequisiteCourse.Code,
                    Units = x.PrerequisiteCourse.Units,
                    GroupNumber = x.PrerequisiteCourse.GroupNumber,
                    Capacity = x.PrerequisiteCourse.Capacity,
                    TeacherName = x.PrerequisiteCourse.TeacherName,
                    Time = x.PrerequisiteCourse.Time,
                    Location = x.PrerequisiteCourse.Location,
                    ExamDateTime = x.PrerequisiteCourse.ExamDateTime
                })
                .ToListAsync();

            return Ok(prerequisites);
        }

        // =====================================
        // POST: api/admin/courses/{courseId}/prerequisites
        // Body: { "prerequisiteCourseId": 12 }
        // =====================================
        [HttpPost]
        public async Task<IActionResult> AddPrerequisite(
            int courseId,
            [FromBody] AddPrerequisiteRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { message = "اطلاعات ارسال‌شده نامعتبر است." });

            try
            {
                await _service.AddAsync(courseId, request.PrerequisiteCourseId);
                return Ok(new { message = "پیش‌نیاز با موفقیت اضافه شد." });
            }
            catch (KeyNotFoundException ex)
            {
                // درس یا پیش‌نیاز پیدا نشده
                return NotFound(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                // حالت‌هایی مثل: خودش بودن، دوطرفه بودن، چرخه
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "خطای داخلی سرور رخ داد." });
            }
        }

        // =====================================
        // DELETE: api/admin/courses/{courseId}/prerequisites/{prerequisiteCourseId}
        // =====================================
        [HttpDelete("{prerequisiteCourseId:int}")]
        public async Task<IActionResult> RemovePrerequisite(
            int courseId,
            int prerequisiteCourseId)
        {
            try
            {
                await _service.RemoveAsync(courseId, prerequisiteCourseId);
                return Ok(new { message = "پیش‌نیاز با موفقیت حذف شد." });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "خطای داخلی سرور رخ داد." });
            }
        }
    }
}
