using Microsoft.AspNetCore.Mvc;
using Orari.DataDbContext;
using Orari.Interfaces;
using Orari.Models;

namespace Orari.Controllers
{
    [Route("api/[controller]")]
    public class CourseController : Controller
    {
        private readonly AppDbContext _context;
        private readonly ICourseRepository _courseRepository;

        public CourseController(AppDbContext context, ICourseRepository courseRepository)
        {
            _context = context;
            _courseRepository = courseRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var courses = await _courseRepository.GetAllCourses();
            return View(courses);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var course = await _courseRepository.GetCourseByIdAsync(id);
            if (course == null)
            {
                return NotFound();
            }
            return View(course);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Courses course)
        {
            if (course == null)
            {
                return BadRequest();
            }
            var createdCourse = await _courseRepository.CreateCourseAsync(course);
            return CreatedAtAction(nameof(GetById), new { id = createdCourse.CId }, createdCourse);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Courses course)
        {
            if (course == null)
            {
                return BadRequest();
            }
            var existingCourse = await _courseRepository.GetCourseByIdAsync(id);
            if (existingCourse == null)
            {
                return NotFound();
            }
            existingCourse.CName = course.CName;
            existingCourse.Credits = course.Credits;
            existingCourse.PId = course.PId;
            existingCourse.Professor = course.Professor;
            existingCourse.Enrollments = course.Enrollments;

            await _courseRepository.UpdateCourseAsync(existingCourse);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var course = await _courseRepository.GetCourseByIdAsync(id);
            if (course == null)
            {
                return NotFound();
            }
            await _courseRepository.DeleteCourseAsync(id);
            return NoContent();
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
