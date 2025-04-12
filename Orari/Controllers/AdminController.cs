using Microsoft.AspNetCore.Mvc;
using Orari.Repository;

namespace Orari.Controllers
{
    public class AdminController : Controller
    {
        private readonly EnrollmentRepository _enrollmentRepository;

        public AdminController(EnrollmentRepository enrollmentRepository)
        {
            _enrollmentRepository = enrollmentRepository;
        }

        public async Task<IActionResult> ManageUsers()
        {
            // Logic to manage users
            return Ok();
        }

        public async Task<IActionResult> ManageCourses()
        {
            // Logic to manage courses
            return Ok();
        }

        public async Task<IActionResult> ManageEnrollments()
        {
            var enrollments = await _enrollmentRepository.GetAllEnrollmentsAsync();
            return Ok(enrollments);
        }
    }
}
