using Microsoft.AspNetCore.Mvc;
using Orari.DataDbContext;
using Orari.Interfaces;
using Orari.Models;
using Orari.Repository;

namespace Orari.Controllers
{
    [Route("api/[controller]")]
    public class StudentController : BaseController
    {
        private readonly AppDbContext _context;
        private readonly IStudentRepository _studentRepository;

        public StudentController(AppDbContext context, IStudentRepository studentRepository, ILogger<BaseController> logger)
            : base(logger)
        {
            _context = context;
            _studentRepository = studentRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var students = await _studentRepository.GetAllStudents();
            return View(students);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var student = await _studentRepository.GetStudentByIdAsync(id);
            if (student == null)
            {
                return NotFound();
            }
            return View(student);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Students student)
        {
            if (student == null)
            {
                return BadRequest();
            }
            var createdStudent = await _studentRepository.CreateStudentAsync(student);
            return CreatedAtAction(nameof(GetById), new { id = createdStudent.SId }, createdStudent);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Students student)
        {
            if (student == null)
            {
                return BadRequest();
            }
            var existingStudent = await _studentRepository.GetStudentByIdAsync(id);
            if (existingStudent == null)
            {
                return NotFound();
            }
            var existingProfesor = await _studentRepository.GetStudentByIdAsync(id);
            if (existingProfesor == null)
            {
                return NotFound();
            }
            existingProfesor.SName = student.SName;
            existingProfesor.SSurname = student.SSurname;
            existingProfesor.SEmail = student.SEmail;
            existingProfesor.SPassword = student.SPassword;
            existingProfesor.SCreatedAt = student.SCreatedAt;
            existingProfesor.SUpdatedAt = DateTime.Now;
            await _studentRepository.UpdateStudentAsync(existingProfesor);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var student = await _studentRepository.GetStudentByIdAsync(id);
            if (student == null)
            {
                return NotFound();
            }
            await _studentRepository.DeleteStudentAsync(id);
            return NoContent();
        }
    }
}
