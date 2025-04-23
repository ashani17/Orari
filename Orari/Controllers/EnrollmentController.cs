using Microsoft.AspNetCore.Mvc;
using Orari.DataDbContext;
using Orari.Interfaces;

namespace Orari.Controllers
{
    public class EnrollmentController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IEnrollmentService _enrollmentService;

        public EnrollmentController(AppDbContext context, IEnrollmentService enrollmentService)
        {
            _context = context;
            _enrollmentService = enrollmentService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var enrollments = await _enrollmentService.GetAllEnrollmentsAsync();
            return Ok(enrollments);
        }

        [HttpPost]
        public async Task<IActionResult> EnrollStudent(int studentId, int courseId)
        {
            var result = await _enrollmentService.EnrollStudentAsync(studentId, courseId);
            if (result)
            {
                return Ok("Student enrolled successfully.");
            }
            return BadRequest("Failed to enroll student.");
        }

        [HttpPost]
        public async Task<IActionResult> UnenrollStudent(int studentId, int courseId)
        {
            var result = await _enrollmentService.UnenrollStudentAsync(studentId, courseId);
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
    }
}
