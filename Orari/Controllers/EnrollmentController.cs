using Microsoft.AspNetCore.Mvc;
using Orari.DataDbContext;
using Orari.Interfaces;

namespace Orari.Controllers
{
    public class EnrollmentController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IEnrollmentRepository _enrollmentRepository;

        public EnrollmentController(AppDbContext context, IEnrollmentRepository enrollmentRepository)
        {
            _context = context;
            _enrollmentRepository = enrollmentRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var enrollments = await _enrollmentRepository.GetAllEnrollmentsAsync();
            return View(enrollments);
        }

        [HttpPost]
        public async Task<IActionResult> EnrollStudent(int studentId, int courseId)
        {
            var result = await _enrollmentRepository.EnrollStudentAsync(studentId, courseId);
            if (result)
            {
                return Ok("Student enrolled successfully.");
            }
            return BadRequest("Failed to enroll student.");
        }

        [HttpPost]
        public async Task<IActionResult> UnenrollStudent(int studentId, int courseId)
        {
            var result = await _enrollmentRepository.UnenrollStudentAsync(studentId, courseId);
            if (result)
            {
                return Ok("Student unenrolled successfully.");
            }
            return BadRequest("Failed to unenroll student.");
        }

        [HttpGet("student/{studentId}/courses")]
        public async Task<IActionResult> GetStudentCourses(int studentId)
        {
            var courses = await _enrollmentRepository.GetStudentCoursesAsync(studentId);
            return View(courses);
        }

        [HttpGet("course/{courseId}/students")]
        public async Task<IActionResult> GetCourseStudents(int courseId)
        {
            var students = await _enrollmentRepository.GetCourseStudentsAsync(courseId);
            return View(students);
        }
    }
}
