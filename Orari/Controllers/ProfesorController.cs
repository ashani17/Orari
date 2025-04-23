using Microsoft.AspNetCore.Mvc;
using Orari.DataDbContext;
using Orari.Interfaces;
using Orari.Models;

namespace Orari.Controllers
{
    [Route("api/[controller]")]
    public class ProfesorController : BaseController
    {
        private readonly AppDbContext _context;
        private readonly IProfesorService _profesorService;

        public ProfesorController(AppDbContext context, IProfesorService profesorService, ILogger<BaseController> logger)
            : base(logger)
        {
            _context = context;
            _profesorService = profesorService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var profesors = await _profesorService.GetAllProfesors();
            return Ok(profesors);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var profesor = await _profesorService.GetProfesorByIdAsync(id);
            if (profesor == null)
            {
                return NotFound();
            }
            return Ok(profesor);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Profesors profesor)
        {
            if (profesor == null)
            {
                return BadRequest();
            }
            var createdProfesor = await _profesorService.CreateProfesorAsync(profesor);
            return CreatedAtAction(nameof(GetById), new { id = createdProfesor.PId }, createdProfesor);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Profesors profesor)
        {
            if (profesor == null)
            {
                return BadRequest();
            }
            var existingProfesor = await _profesorService.GetProfesorByIdAsync(id);
            if (existingProfesor == null)
            {
                return NotFound();
            }
            existingProfesor.PName = profesor.PName;
            existingProfesor.PSurname = profesor.PSurname;
            existingProfesor.PEmail = profesor.PEmail;
            existingProfesor.PPassword = profesor.PPassword;
            existingProfesor.PPhone = profesor.PPhone;
            existingProfesor.PCreatedAt = profesor.PCreatedAt;
            existingProfesor.PUpdatedAt = DateTime.Now;
            await _profesorService.UpdateProfesorAsync(existingProfesor);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var profesor = await _profesorService.GetProfesorByIdAsync(id);
            if (profesor == null)
            {
                return NotFound();
            }
            await _profesorService.DeleteProfesorAsync(id);
            return NoContent();
        }
    }
}
