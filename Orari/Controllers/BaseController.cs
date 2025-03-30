using Microsoft.AspNetCore.Mvc;

namespace Orari.Controllers
{
    public class BaseController : Controller
    {
        private readonly ILogger<BaseController> _logger;

        public BaseController(ILogger<BaseController> logger)
        {
            _logger = logger;
        }

        protected IActionResult HandleException(Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return StatusCode(500, "An unexpected error occurred. Please try again later.");
        }

        protected IActionResult NotFoundResponse(string message)
        {
            return NotFound(new { Message = message });
        }

        protected IActionResult BadRequestResponse(string message)
        {
            return BadRequest(new { Message = message });
        }

        protected string GetCurrentUserId()
        {
            return User?.Identity?.Name ?? "Anonymous";
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
