using Microsoft.AspNetCore.Mvc;
using Orari.DataDbContext;
using Orari.DTO.CoursesDTO;
using Orari.Interfaces;
using Orari.Models;
using Orari.Services;

namespace Orari.Controllers
{
    [Route("api/[controller]")]
    public class CourseController : Controller
    {
       
        private readonly ICourseService _courseService;

        public CourseController(ICourseService courseService)
        {
            
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
        public async Task<IActionResult> Create([FromBody] PostCourseDTO course)
        {
            if (course == null)
            {
                return BadRequest();
            }

            var courseModel = new Courses
            {
                CName = course.CName,
                Credits = course.Credits,
                PId = course.Profesor.PId,
                Profesor = course.Profesor.PEmail // Fix: Set the required 'Profesor' property
            };

            var createdCourse = await _courseService.CreateCourseAsync(courseModel);
            return CreatedAtAction(nameof(GetById), new { id = createdCourse.CId }, createdCourse);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] PutCourseDTO course)
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

            // Fix: Assign the 'PName' property of 'Profesors' to the 'Profesor' string field
            existingCourse.PId = course.Profesor.PId;

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
