using Microsoft.AspNetCore.Authorization;
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

        [AllowAnonymous]
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<GetDelCourseDTO>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllCourses()
        {
            var courses = await _courseService.GetAllCoursesAsync();
            
            // Map to DTOs to avoid circular references
            var courseDtos = courses.Select(c => new GetDelCourseDTO
            {
                CId = c.CId,
                CName = c.CName,
                Credits = c.Credits,
                PId = c.PId,
                Profesor = c.Profesor
            });
            
            return Ok(courseDtos);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Courses), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetCourseById([FromRoute] int id)
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
        public async Task<IActionResult> CreateCourse([FromBody] PostCourseDTO dto)
        {
            try
            {
                var course = new Courses
                {
                    CName = dto.CName,
                    Credits = dto.Credits,
                    PId = dto.PId,
                    Profesor = dto.Profesor,
                    Enrollments = new List<Enrollments>(),
                    StudyProgramCourse = new List<StudyProgramCourse>()
                };

                var createdCourse = await _courseService.CreateCourseAsync(course);

                return CreatedAtAction(nameof(GetCourseById), new { id = createdCourse.CId }, createdCourse);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(Courses), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateCourse(int id, [FromBody] PutCourseDTO putCourseDTO)
        {
            try
            {
                var existingCourse = await _courseService.GetCourseByIdAsync(id);
                if (existingCourse == null)
                {
                    return NotFound("Course not found");
                }

                existingCourse.CName = putCourseDTO.CName;
                existingCourse.Credits = putCourseDTO.Credits;
                existingCourse.Profesor = putCourseDTO.ProfessorName ?? existingCourse.Profesor;

                var updatedCourse = await _courseService.UpdateCourseAsync(existingCourse);
                return Ok(updatedCourse);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
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

        [HttpGet("study-program/{studyProgramId}")]
        [ProducesResponseType(typeof(IEnumerable<Courses>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetCoursesByStudyProgram(int studyProgramId)
        {
            // Study program functionality removed. Endpoint kept for compatibility.
            return Ok(new List<Courses>());
        }
    }
}
