using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UniversityRegistration.Api.Data;
using UniversityRegistration.Api.Models;
using UniversityRegistration.Api.Models.DTOs;

namespace UniversityRegistration.Api.Controllers
{
    [ApiController]
    [Route("api/admin/courses/{courseId:int}/prerequisites")]
    [Authorize(Roles = "Admin")]
    public class AdminCoursePrerequisitesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AdminCoursePrerequisitesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/admin/courses/{courseId}/prerequisites
        [HttpGet]
        public async Task<IActionResult> GetPrerequisites(int courseId)
        {
            var exists = await _context.Courses.AnyAsync(c => c.Id == courseId);
            if (!exists)
                return NotFound("Course not found.");

            var prerequisites = await _context.CoursePrerequisites
                .Where(x => x.CourseId == courseId)
                .Select(x => new
                {
                    x.PrerequisiteCourseId
                })
                .ToListAsync();

            return Ok(prerequisites);
        }

        // POST: api/admin/courses/{courseId}/prerequisites
        [HttpPost]
        public async Task<IActionResult> AddPrerequisite(
            int courseId,
            [FromBody] AddPrerequisiteRequest request)
        {
            if (courseId == request.PrerequisiteCourseId)
                return BadRequest("A course cannot be its own prerequisite.");

            var courseExists = await _context.Courses.AnyAsync(c => c.Id == courseId);
            var prereqExists = await _context.Courses.AnyAsync(c => c.Id == request.PrerequisiteCourseId);

            if (!courseExists || !prereqExists)
                return NotFound("Course or prerequisite course not found.");

            var alreadyExists = await _context.CoursePrerequisites.AnyAsync(x =>
                x.CourseId == courseId &&
                x.PrerequisiteCourseId == request.PrerequisiteCourseId);

            if (alreadyExists)
                return Conflict("This prerequisite already exists.");

            var prerequisite = new CoursePrerequisite
            {
                CourseId = courseId,
                PrerequisiteCourseId = request.PrerequisiteCourseId
            };

            _context.CoursePrerequisites.Add(prerequisite);
            await _context.SaveChangesAsync();

            return Ok(prerequisite);
        }

        // DELETE: api/admin/courses/{courseId}/prerequisites/{prerequisiteCourseId}
        [HttpDelete("{prerequisiteCourseId:int}")]
        public async Task<IActionResult> RemovePrerequisite(
            int courseId,
            int prerequisiteCourseId)
        {
            var entity = await _context.CoursePrerequisites.FindAsync(courseId, prerequisiteCourseId);
            if (entity == null)
                return NotFound("Prerequisite not found.");

            _context.CoursePrerequisites.Remove(entity);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
