using Microsoft.AspNetCore.Mvc;
using Orari.DataDbContext;
using Orari.DTO.EnrollmentDTO;
using Orari.Interfaces;

namespace Orari.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EnrollmentController : Controller
    {
        
        private readonly IEnrollmentService _enrollmentService;

        public EnrollmentController(IEnrollmentService enrollmentService)
        {
            
            _enrollmentService = enrollmentService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var enrollments = await _enrollmentService.GetAllEnrollmentsAsync();
            return Ok(enrollments);
        }

        [HttpPost("enroll")]
        public async Task<IActionResult> EnrollStudent([FromBody] EnrollmentDTO dto)
        {
            var result = await _enrollmentService.EnrollStudentAsync(dto.SId, dto.CId);
            if (result)
            {
                return Ok("Student enrolled successfully.");
            }
            return BadRequest("Failed to enroll student.");
        }

        [HttpPost("unenroll")]
        public async Task<IActionResult> UnenrollStudent([FromBody] EnrollmentDTO dto)
        {
            var result = await _enrollmentService.UnenrollStudentAsync(dto.SId, dto.CId);
            if (result)
            {
                return Ok("Student unenrolled successfully.");
            }
            return BadRequest("Failed to unenroll student.");
        }

        [HttpGet("student/{studentId}/courses")]
        public async Task<IActionResult> GetStudentCourses(int studentId)
        {
            var courses = await _enrollmentService.GetStudentCoursesAsync(studentId);
            return Ok(courses);
        }

        [HttpGet("course/{courseId}/students")]
        public async Task<IActionResult> GetCourseStudents(int courseId)
        {
            var students = await _enrollmentService.GetCourseStudentsAsync(courseId);
            return Ok(students);
        }

        [HttpGet("student/email/{email}/courses")]
        public async Task<IActionResult> GetStudentCoursesByEmail(string email)
        {
            try
            {
                var courses = await _enrollmentService.GetStudentCoursesByEmailAsync(email);
                return Ok(courses);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("course/name/{courseName}/students")]
        public async Task<IActionResult> GetCourseStudentsByName(string courseName)
        {
            try
            {
                var students = await _enrollmentService.GetCourseStudentsByNameAsync(courseName);
                return Ok(students);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
