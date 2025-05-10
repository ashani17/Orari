using Microsoft.AspNetCore.Mvc;
using Orari.DataDbContext;
using Orari.DTO.StudentDTO;
using Orari.Interfaces;
using Orari.Models;
using Orari.Services;

namespace Orari.Controllers
{
    [Route("api/[controller]")]
    public class StudentController : BaseController
    {
        
        private readonly IStudentService _studentService;

        public StudentController(IStudentService studentService, ILogger<BaseController> logger)
            : base(logger)
        {
          
            _studentService = studentService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var students = await _studentService.GetAllStudents();
            return Ok(students);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var student = await _studentService.GetStudentByIdAsync(id);
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
        public async Task<IActionResult> Delete(int id)
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
