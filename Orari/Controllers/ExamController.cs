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
        private readonly IExamService _examService;

        public ExamController(AppDbContext context, IExamService examService)
        {
            _context = context;
            _examService = examService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var exams = await _examService.GetAllExams();
            return Ok(exams);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var exam = await _examService.GetExamByIdAsync(id);
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
            var createdExam = await _examService.CreateExamAsync(exam);
            return CreatedAtAction(nameof(GetById), new { id = createdExam.EId }, createdExam);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Exams exam)
        {
            if (exam == null)
            {
                return BadRequest();
            }
            var existingExam = await _examService.GetExamByIdAsync(id);
            if (existingExam == null)
            {
                return NotFound();
            }
            existingExam.ExamName = exam.ExamName;
            existingExam.ExamDate = exam.ExamDate;
            existingExam.Schedule = exam.Schedule;
            existingExam.Profesor = exam.Profesor;
            existingExam.Course = exam.Course;

            await _examService.UpdateExamAsync(existingExam);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var exam = await _examService.GetExamByIdAsync(id);
            if (exam == null)
            {
                return NotFound();
            }
            await _examService.DeleteExamAsync(id);
            return NoContent();
        }
    }
}
