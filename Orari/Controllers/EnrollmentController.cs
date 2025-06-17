using Microsoft.AspNetCore.Mvc;
using Orari.DataDbContext;
using Orari.DTO.EnrollmentDTO;
using Orari.DTO.CoursesDTO;
using Orari.Interfaces;
using Orari.Models;

namespace Orari.Controllers
{
    [ApiController]
    [Route("api/enrollments")]
    [Produces("application/json")]
    public class EnrollmentController : Controller
    {
        
        private readonly IEnrollmentService _enrollmentService;

        public EnrollmentController(IEnrollmentService enrollmentService)
        {
            
            _enrollmentService = enrollmentService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<EnrollmentWithDetailsDTO>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Index()
        {
            var enrollments = await _enrollmentService.GetAllEnrollmentsAsync();
            
            // Map to DTOs to avoid circular references
            var enrollmentDtos = enrollments.Select(e => new EnrollmentWithDetailsDTO
            {
                EId = e.EId,
                StudentId = e.StudentId,
                StudentName = $"{e.Student?.FirstName} {e.Student?.LastName}".Trim(),
                StudentEmail = e.Student?.Email ?? string.Empty,
                CId = e.CId,
                CourseName = e.Courses?.CName ?? string.Empty,
                CourseCredits = e.Courses?.Credits ?? 0,
                ProfessorName = e.Courses?.Profesor ?? string.Empty
            });
            
            return Ok(enrollmentDtos);
        }

        [HttpPost("enroll")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> EnrollStudent([FromBody] EnrollmentDto dto)
        {
            var result = await _enrollmentService.EnrollStudentAsync(dto.StudentId, dto.CId);
            if (result)
            {
                return Ok("Student enrolled successfully.");
            }
            return BadRequest("Failed to enroll student.");
        }

        [HttpPost("unenroll")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UnenrollStudent([FromBody] EnrollmentDto dto)
        {
            var result = await _enrollmentService.UnenrollStudentAsync(dto.StudentId, dto.CId);
            if (result)
            {
                return Ok("Student unenrolled successfully.");
            }
            return BadRequest("Failed to unenroll student.");
        }

        [HttpGet("student/{studentId}/courses")]
        [ProducesResponseType(typeof(IEnumerable<GetDelCourseDTO>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetStudentCourses([FromRoute] string studentId)
        {
            var courses = await _enrollmentService.GetStudentCoursesAsync(studentId);
            
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

        [HttpGet("course/{courseId}/students")]
        [ProducesResponseType(typeof(IEnumerable<object>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetCourseStudents([FromRoute] int courseId)
        {
            var students = await _enrollmentService.GetCourseStudentsAsync(courseId);
            
            // Map to simple objects to avoid circular references
            var studentDtos = students.Select(s => new
            {
                Id = s.Id,
                Email = s.Email,
                FirstName = s.FirstName,
                LastName = s.LastName,
                UserName = s.UserName
            });
            
            return Ok(studentDtos);
        }

        [HttpGet("student/email/{email}/courses")]
        [ProducesResponseType(typeof(IEnumerable<GetDelCourseDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetStudentCoursesByEmail([FromRoute] string email)
        {
            try
            {
                var courses = await _enrollmentService.GetStudentCoursesByEmailAsync(email);
                
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
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("course/name/{courseName}/students")]
        [ProducesResponseType(typeof(IEnumerable<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetCourseStudentsByName([FromRoute] string courseName)
        {
            try
            {
                var students = await _enrollmentService.GetCourseStudentsByNameAsync(courseName);
                
                // Map to simple objects to avoid circular references
                var studentDtos = students.Select(s => new
                {
                    Id = s.Id,
                    Email = s.Email,
                    FirstName = s.FirstName,
                    LastName = s.LastName,
                    UserName = s.UserName
                });
                
                return Ok(studentDtos);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
