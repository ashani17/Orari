using Microsoft.AspNetCore.Mvc;
using Orari.DataDbContext;
using Orari.DTO.EnrollmentsDTO;
using Orari.Interfaces;

namespace Orari.Controllers
{
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

        [HttpPost]
        public async Task<IActionResult> EnrollStudent([FromBody] EnrollmentDTO dto)
        {
            var result = await _enrollmentService.EnrollStudentAsync(dto.CId, dto.SId);
            if (result)
            {
                return Ok("Student enrolled successfully.");
            }
            return BadRequest("Failed to enroll student.");
        }

        [HttpPost]
        public async Task<IActionResult> UnenrollStudent([FromBody] EnrollmentDTO dto)
        {
            var result = await _enrollmentService.UnenrollStudentAsync(dto.CId, dto.SId);
            if (result)
            {
                return Ok("Student unenrolled successfully.");
            }
            return BadRequest("Failed to unenroll student.");
        }

        [HttpGet("student/{studentId}/courses")]
        public async Task<IActionResult> GetStudentCourses([FromBody] EnrollmentDTO dto)
        {
            var courses = await _enrollmentService.GetStudentCoursesAsync(dto.SId);
            return Ok(courses);
        }

        [HttpGet("course/{courseId}/students")]
        public async Task<IActionResult> GetCourseStudents([FromBody] EnrollmentDTO dto)
        {
            var students = await _enrollmentService.GetCourseStudentsAsync(dto.CId);
            return Ok(students);
        }
    }
}
