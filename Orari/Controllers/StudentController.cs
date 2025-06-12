using Microsoft.AspNetCore.Mvc;
using Orari.DataDbContext;
using Orari.DTO.StudentDTO;
using Orari.Interfaces;
using Orari.Models;
using Orari.Services;

namespace Orari.Controllers
{
    [ApiController]
    [Route("api/students")]
    [Produces("application/json")]
    public class StudentController : Controller
    {
        private readonly IStudentService _studentService;
        private readonly ILogger<StudentController> _logger;

        public StudentController(IStudentService studentService, ILogger<StudentController> logger)
        {
            _studentService = studentService;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Students>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            var students = await _studentService.GetAllStudents();
            return Ok(students);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Students), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById([FromRoute] string id)
        {
            var student = await _studentService.GetStudentByIdAsync(id);
            if (student == null)
            {
                return NotFound();
            }
            return Ok(student);
        }

        [HttpPost]
        [ProducesResponseType(typeof(Students), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] PostStudentDTO student)
        {
            if (student == null)
            {
                return BadRequest();
            }

            // Map PostStudentDTO to Students model
            var studentModel = new Students
            {
                SName = student.SName,
                SSurname = student.SSurname,
                SEmail = student.SEmail,
                SPassword = student.SPassword,
                SCreatedAt = DateTime.UtcNow,
            };

            var createdStudent = await _studentService.CreateStudentAsync(studentModel);
            return CreatedAtAction(nameof(GetById), new { id = createdStudent.Id }, createdStudent);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(Students), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update([FromRoute] string id, [FromBody] PutStudentDTO student)
        {
            if (student == null)
            {
                return BadRequest();
            }
            var existingStudent = await _studentService.GetStudentByIdAsync(id);
            if (existingStudent == null)
            {
                return NotFound();
            }
            existingStudent.SName = student.SName;
            existingStudent.SSurname = student.SSurname;
            existingStudent.SEmail = student.SEmail;
            existingStudent.SPassword = student.SPassword;

            await _studentService.UpdateStudentAsync(existingStudent);
            return NoContent();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete([FromRoute] string id)
        {
            var student = await _studentService.GetStudentByIdAsync(id);
            if (student == null)
            {
                return NotFound();
            }
            await _studentService.DeleteStudentAsync(id);
            return NoContent();
        }
    }
}
