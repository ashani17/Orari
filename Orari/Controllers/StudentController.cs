using Microsoft.AspNetCore.Mvc;
using Orari.DataDbContext;
using Orari.DTO.StudentDTO;
using Orari.Interfaces;
using Orari.Models;
using Orari.Services;

namespace Orari.Controllers
{
    [Route("api/[controller]")]
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
        public async Task<IActionResult> GetAll()
        {
            var students = await _studentService.GetAllStudents();
            return Ok(students);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromBody] GetDelStudentDTO dto)
        {
            var student = await _studentService.GetStudentByIdAsync(dto.SId);
            if (student == null)
            {
                return NotFound();
            }
            return Ok(student);
        }

        [HttpPost]
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
                Enrollments = student.Enrollments,
                SCreatedAt = DateTime.UtcNow,
            };

            var createdStudent = await _studentService.CreateStudentAsync(studentModel);
            return CreatedAtAction(nameof(GetById), new { id = createdStudent.SId }, createdStudent);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] PutStudentDTO student)
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
        public async Task<IActionResult> Delete([FromBody] GetDelStudentDTO dto)
        {
            var student = await _studentService.GetStudentByIdAsync(dto.SId);
            if (student == null)
            {
                return NotFound();
            }
            await _studentService.DeleteStudentAsync(dto.SId);
            return NoContent();
        }
    }
}
