using Microsoft.AspNetCore.Mvc;
using Orari.Repository;
using Orari.Services;

namespace Orari.Controllers
{
    public class AdminController : Controller
    {
        private readonly EnrollmentService _enrollmentService;

        public AdminController(EnrollmentService enrollmentService)
        {
            _enrollmentService = enrollmentService;
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
            var enrollments = await _enrollmentService.GetAllEnrollmentsAsync();
            return Ok(enrollments);
        }
    }
}
