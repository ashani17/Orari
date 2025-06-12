using Microsoft.AspNetCore.Mvc;
using Orari.Models;
using Orari.Repository;
using Orari.Services;

namespace Orari.Controllers
{
    [ApiController]
    [Route("api/admin")]
    [Produces("application/json")]
    public class AdminController : Controller
    {
        private readonly EnrollmentService _enrollmentService;

        public AdminController(EnrollmentService enrollmentService)
        {
            _enrollmentService = enrollmentService;
        }

        [HttpGet("users")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> ManageUsers()
        {
            // Logic to manage users
            return Ok();
        }

        [HttpGet("courses")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> ManageCourses()
        {
            // Logic to manage courses
            return Ok();
        }

        [HttpGet("enrollments")]
        [ProducesResponseType(typeof(IEnumerable<Enrollments>), StatusCodes.Status200OK)]
        public async Task<IActionResult> ManageEnrollments()
        {
            var enrollments = await _enrollmentService.GetAllEnrollmentsAsync();
            return Ok(enrollments);
        }
    }
}
