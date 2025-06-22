using Microsoft.AspNetCore.Mvc;
using Orari.DataDbContext;
using Orari.DTO.ExamDTO;
using Orari.Interfaces;
using Orari.Models;

namespace Orari.Controllers
{
    [Route ("api/exams")]
    public class ExamController : Controller
    {
        
        private readonly IExamService _examService;

        public ExamController(IExamService examService)
        {
            
            _examService = examService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Exams>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllExams()
        {
            var exams = await _examService.GetAllExamsAsync();
            return Ok(exams);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Exams), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetExamById(int id)
        {
            var exam = await _examService.GetExamByIdAsync(id);
            if (exam == null)
            {
                return NotFound("Exam not found");
            }
            return Ok(exam);
        }

        [HttpPost]
        [ProducesResponseType(typeof(Exams), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateExam([FromBody] Exams exam)
        {
            try
            {
                var createdExam = await _examService.CreateExamAsync(exam);
                return CreatedAtAction(nameof(GetExamById), new { id = createdExam.EId }, createdExam);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(Exams), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateExam(int id, [FromBody] Exams exam)
        {
            try
            {
                var existingExam = await _examService.GetExamByIdAsync(id);
                if (existingExam == null)
                {
                    return NotFound("Exam not found");
                }

                existingExam.ExamName = exam.ExamName;
                existingExam.ExamDate = exam.ExamDate;
                existingExam.StartTime = exam.StartTime;
                existingExam.EndTime = exam.EndTime;
                existingExam.CId = exam.CId;
                existingExam.SCId = exam.SCId;
                existingExam.ProfessorId = exam.ProfessorId;
                existingExam.RId = exam.RId;

                var updatedExam = await _examService.UpdateExamAsync(existingExam);
                return Ok(updatedExam);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteExam(int id)
        {
            var exam = await _examService.GetExamByIdAsync(id);
            if (exam == null)
            {
                return NotFound("Exam not found");
            }
            await _examService.DeleteExamAsync(id);
            return NoContent();
        }
    }
}
