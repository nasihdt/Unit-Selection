using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UniversityRegistration.Api.Models.DTOs;
using UniversityRegistration.Api.Services.Interfaces;

namespace UniversityRegistration.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CourseController : ControllerBase
    {
        private readonly ICourseService _service;

        public CourseController(ICourseService service)
        {
            _service = service;
        }

        // ==========================
        // GET: api/course
        // ==========================
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAll([FromQuery] CourseQueryParameters query)
        {
            var courses = await _service.GetFilteredAsync(query);
            return Ok(courses);
        }

        // ==========================
        // GET: api/course/{id}
        // ==========================
        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetById(int id)
        {
            var course = await _service.GetByIdAsync(id);
            if (course == null)
                return NotFound(new { message = "درس یافت نشد" });

            return Ok(course);
        }

        // ==========================
        // POST: api/course
        // ==========================
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Add([FromBody] CreateCourseRequest dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var created = await _service.AddAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
            }
            catch (ArgumentException ex)
            {
                // خطاهای اعتبارسنجی زمان یا روز هفته
                return BadRequest(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                // تداخل مکانی یا تکراری بودن کد+گروه
                return Conflict(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = ex.Message,
                    detail = ex.InnerException?.Message
                });
            }

        }

        // ==========================
        // PATCH: api/course/{id}
        // ==========================
        [HttpPatch("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Patch(int id, [FromBody] PatchCourseRequest dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var success = await _service.PatchAsync(id, dto);
                if (!success)
                    return NotFound(new { message = "درس یافت نشد" });

                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "خطای داخلی سرور رخ داده است" });
            }
        }

        // ==========================
        // PUT: api/course/{id}
        // ==========================
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateCourseRequest dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var success = await _service.UpdateAsync(id, dto);
                if (!success)
                    return NotFound(new { message = "درس یافت نشد" });

                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "خطای داخلی سرور رخ داده است" });
            }
        }

        // ==========================
        // GET: api/course/{id}/delete-info
        // ==========================
        [HttpGet("{id}/delete-info")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetDeleteInfo(int id)
        {
            var info = await _service.GetDeleteInfoAsync(id);
            if (info == null)
                return NotFound(new { message = "درس یافت نشد" });

            return Ok(info);
        }

        // ==========================
        // DELETE: api/course/{id}
        // ==========================
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var success = await _service.DeleteAsync(id);
                if (!success)
                    return NotFound(new { message = "درس یافت نشد" });

                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                // اگر خواستی بعداً منطق خاص برای حذف (مثلاً وابستگی‌ها) اضافه کنی
                return Conflict(new { message = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "خطای داخلی سرور رخ داده است" });
            }
        }
    }
}
