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
                Profesor = c.Profesor,
                StudyProgramId = c.StudyProgramCourse.FirstOrDefault()?.SPId,
                StudyProgramName = c.StudyProgramCourse.FirstOrDefault()?.StudyProgram?.SPName
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

                // Add StudyProgramCourse relationship
                if (dto.StudyProgramId > 0)
                {
                    using (var scope = HttpContext.RequestServices.CreateScope())
                    {
                        var db = (Orari.DataDbContext.AppDbContext)scope.ServiceProvider.GetService(typeof(Orari.DataDbContext.AppDbContext));
                        var spc = new StudyProgramCourse
                        {
                            SPId = dto.StudyProgramId,
                            CId = createdCourse.CId
                        };
                        db.StudyProgramCourses.Add(spc);
                        db.SaveChanges();
                    }
                }

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

                // Update StudyProgramCourse relationship
                if (putCourseDTO.StudyProgramId > 0)
                {
                    // Remove old relationships
                    existingCourse.StudyProgramCourse.Clear();
                    // Add new relationship
                    existingCourse.StudyProgramCourse.Add(new Orari.Models.StudyProgramCourse
                    {
                        SPId = putCourseDTO.StudyProgramId,
                        CId = existingCourse.CId
                    });
                }

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
            try
            {
                // Since we removed study program functionality, return empty list for now
                // This endpoint can be removed or updated based on requirements
                return Ok(new List<Courses>());
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
