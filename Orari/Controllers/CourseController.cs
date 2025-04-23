using Microsoft.AspNetCore.Mvc;
using Orari.DataDbContext;
using Orari.Interfaces;
using Orari.Models;
using Orari.Services;

namespace Orari.Controllers
{
    [Route("api/[controller]")]
    public class CourseController : Controller
    {
        private readonly AppDbContext _context;
        private readonly ICourseService _courseService;

        public CourseController(AppDbContext context, ICourseService courseService)
        {
            _context = context;
            _courseService = courseService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var courses = await _courseService.GetAllCourses();
            return Ok(courses);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var course = await _courseService.GetCourseByIdAsync(id);
            if (course == null)
            {
                return NotFound();
            }
            return Ok(course);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Courses course)
        {
            if (course == null)
            {
                return BadRequest();
            }

            // Pass the required 'CName' parameter along with the course object
            var createdCourse = await _courseService.CreateCourseAsync(course, course.CName);
            return CreatedAtAction(nameof(GetById), new { id = createdCourse.CId }, createdCourse);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Courses course)
        {
            if (course == null)
            {
                return BadRequest();
            }
            var existingCourse = await _courseService.GetCourseByIdAsync(id);
            if (existingCourse == null)
            {
                return NotFound();
            }

            // Update the properties of the existing course
            existingCourse.CName = course.CName;
            existingCourse.Credits = course.Credits;
            existingCourse.PId = course.PId;
            existingCourse.Professor = course.Professor;
            existingCourse.Enrollments = course.Enrollments;

            // Pass both the id and the updated course object to the UpdateCourseAsync method
            await _courseService.UpdateCourseAsync(id, existingCourse);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var course = await _courseService.GetCourseByIdAsync(id);
            if (course == null)
            {
                return NotFound();
            }
            await _courseService.DeleteCourseAsync(id);
            return NoContent();
        }

    }
}
