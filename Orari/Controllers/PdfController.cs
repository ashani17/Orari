using Microsoft.AspNetCore.Mvc;
using Orari.Services;
using System.ComponentModel.DataAnnotations;

namespace Orari.Controllers
{
    [ApiController]
    [Route("api/pdf")]
    public class PdfController : ControllerBase
    {
        private readonly IPdfGenerateService _pdfGenerateService;

        public PdfController(IPdfGenerateService pdfGenerateService)
        {
            _pdfGenerateService = pdfGenerateService;
        }

        public class GeneratePdfRequest
        {
            [Required]
            public string Title { get; set; } = string.Empty;
            [Required]
            public string Content { get; set; } = string.Empty;
            public string? Filename { get; set; }
        }

        [HttpPost("generate")]
        public IActionResult GeneratePdf([FromBody] GeneratePdfRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var pdfBytes = _pdfGenerateService.GenerateSchedulePdf(request.Title, request.Content);
            
            // Generate unique filename with timestamp
            var timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
            var filename = !string.IsNullOrEmpty(request.Filename) 
                ? $"{request.Filename}_{timestamp}_Orari.pdf"
                : $"Schedule_{timestamp}_Orari.pdf";
            
            return File(pdfBytes, "application/pdf", filename);
        }
    }
} 