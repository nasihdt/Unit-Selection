using Microsoft.AspNetCore.Mvc;
using UniversityRegistration.Api.Models;
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

        // GET: api/course
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var courses = await _service.GetAllAsync();
            return Ok(courses);
        }

        // GET: api/course/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var course = await _service.GetByIdAsync(id);
            if (course == null)
                return NotFound();

            return Ok(course);
        }

        // POST: api/course
        [HttpPost]
        public async Task<IActionResult> Add(Course course)
        {
            var created = await _service.AddAsync(course);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        // PUT: api/course/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Course course)
        {
            var success = await _service.UpdateAsync(id, course);
            if (!success)
                return NotFound();

            return NoContent();
        }

        // DELETE: api/course/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _service.DeleteAsync(id);
            if (!success)
                return NotFound();

            return NoContent();
        }
    }
}
