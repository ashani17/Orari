using Microsoft.AspNetCore.Mvc;
using Orari.DataDbContext;
using Orari.DTO.ProfesorDTO;
using Orari.Interfaces;
using Orari.Models;

namespace Orari.Controllers
{
    [ApiController]
    [Route("api/professors")]
    [Produces("application/json")]
    public class ProfesorController : Controller
    {
        private readonly IProfesorService _profesorService;
        private readonly ILogger<ProfesorController> _logger;

        public ProfesorController(IProfesorService profesorService, ILogger<ProfesorController> logger)
        {
            _profesorService = profesorService;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Profesors>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            var profesors = await _profesorService.GetAllProfesors();
            return Ok(profesors);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Profesors), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var profesor = await _profesorService.GetProfesorByIdAsync(id);
            if (profesor == null)
            {
                return NotFound();
            }
            return Ok(profesor);
        }

        [HttpPost]
        [ProducesResponseType(typeof(Profesors), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] PostProfesorDTO profesor)
        {
            if (profesor == null)
            {
                return BadRequest();
            }
            // Create a new instance of the Profesors model
            var profesorModel = new Profesors
            {
                PName = profesor.PName,
                PSurname = profesor.PSurname,
                PEmail = profesor.PEmail,
                PPassword = profesor.PPassword,
                PPhone = profesor.PPhone,
                PSubject = profesor.PSubject,
                Availability = profesor.Availability,
                SpecialRequirements = profesor.SpecialRequirements,
                PCreatedAt = DateTime.Now,
            };
            var createdProfesor = await _profesorService.CreateProfesorAsync(profesorModel);
            return CreatedAtAction(nameof(GetById), new { id = createdProfesor.PId }, createdProfesor);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] PutProfesorDTO profesor)
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
            existingProfesor.PUpdatedAt = DateTime.Now;
            await _profesorService.UpdateProfesorAsync(existingProfesor);
            return NoContent();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete([FromRoute] int id)
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
