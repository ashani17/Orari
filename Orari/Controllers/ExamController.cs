using Microsoft.AspNetCore.Mvc;
using Orari.DataDbContext;
using Orari.Interfaces;
using Orari.Models;

namespace Orari.Controllers
{
    [Route ("api/exams")]
    public class ExamController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IExamRepository _examRepository;

        public ExamController(AppDbContext context, IExamRepository examRepository)
        {
            _context = context;
            _examRepository = examRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var exams = await _examRepository.GetAllExams();
            return Ok(exams);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var exam = await _examRepository.GetExamByIdAsync(id);
            if (exam == null)
            {
                return NotFound();
            }
            return Ok(exam);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Exams exam)
        {
            if (exam == null)
            {
                return BadRequest();
            }
            var createdExam = await _examRepository.CreateExamAsync(exam);
            return CreatedAtAction(nameof(GetById), new { id = createdExam.EId }, createdExam);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Exams exam)
        {
            if (exam == null)
            {
                return BadRequest();
            }
            var existingExam = await _examRepository.GetExamByIdAsync(id);
            if (existingExam == null)
            {
                return NotFound();
            }
            existingExam.ExamName = exam.ExamName;
            existingExam.ExamDate = exam.ExamDate;
            existingExam.Schedule = exam.Schedule;
            existingExam.Profesor = exam.Profesor;
            existingExam.Course = exam.Course;

            await _examRepository.UpdateExamAsync(existingExam);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var exam = await _examRepository.GetExamByIdAsync(id);
            if (exam == null)
            {
                return NotFound();
            }
            await _examRepository.DeleteExamAsync(id);
            return NoContent();
        }
    }
}
