using Microsoft.AspNetCore.Mvc;
using Orari.DataDbContext;
using Orari.DTO.CoursesDTO;
using Orari.Interfaces;
using Orari.Models;
using Orari.Services;

namespace Orari.Controllers
{
    [ApiController]
    [Route("api/courses")]
    [Produces("application/json")]
    public class CourseController : Controller
    {
        private readonly ICourseService _courseService;

        public CourseController(ICourseService courseService)
        {
            _courseService = courseService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Courses>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            var courses = await _courseService.GetAllCourses();
            return Ok(courses);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Courses), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var course = await _courseService.GetCourseByIdAsync(id);
            if (course == null)
            {
                return NotFound();
            }
            return Ok(course);
        }

        [HttpPost]
        [ProducesResponseType(typeof(Courses), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
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
                Profesor = course.Profesor.PEmail
            };

            try
            {
                var createdCourse = await _courseService.CreateCourseAsync(courseModel, course.StudyProgramId);
                return CreatedAtAction(nameof(GetById), new { id = createdCourse.CId }, createdCourse);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] PutCourseDTO course)
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

            existingCourse.CName = course.CName;
            existingCourse.Credits = course.Credits;
            existingCourse.PId = course.Profesor.PId;

            await _courseService.UpdateCourseAsync(id, existingCourse);
            return NoContent();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var course = await _courseService.GetCourseByIdAsync(id);
            if (course == null)
            {
                return NotFound();
            }
            await _courseService.DeleteCourseAsync(id);
            return NoContent();
        }

        [HttpGet("studyprogram/{studyProgramId}")]
        [ProducesResponseType(typeof(IEnumerable<Courses>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetCoursesByStudyProgram([FromRoute] int studyProgramId)
        {
            try
            {
                var courses = await _courseService.GetCoursesByStudyProgramAsync(studyProgramId);
                return Ok(courses);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
