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
        public async Task<IActionResult> GetAll()
        {
            var exams = await _examService.GetAllExams();
            return Ok(exams);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromBody] GetDelExamDTO dto)
        {
            var exam = await _examService.GetExamByIdAsync(dto.EId);
            if (exam == null)
            {
                return NotFound();
            }
            return Ok(exam);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] PostExamDTO exam)
        {
            if (exam == null)
            {
                return BadRequest();
            }

            // Map PostExamDTO to Exams model
            var examModel = new Exams
            {
                ExamName = exam.ExamName,
                ExamDate = exam.ExamDate,
                StartTime = exam.StartTime,
                EndTime = exam.EndTime,
                SCId = exam.ScheduleId,
                PId = exam.ProfesorId,
                CId = exam.CourseId,
                RId = exam.RoomId,
            };

            // Pass the mapped Exams model to the service
            var createdExam = await _examService.CreateExamAsync(examModel);
            return CreatedAtAction(nameof(GetById), new { id = createdExam.EId }, createdExam);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] PutExamDTO exam)
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
            existingExam.StartTime = exam.StartTime;
            existingExam.EndTime = exam.EndTime;
            existingExam.SCId = exam.ScheduleId;
            existingExam.PId = exam.ProfesorId;
            existingExam.CId = exam.CourseId;

            await _examService.UpdateExamAsync(existingExam);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromBody] GetDelExamDTO dto)
        {
            var exam = await _examService.GetExamByIdAsync(dto.EId);
            if (exam == null)
            {
                return NotFound();
            }
            await _examService.DeleteExamAsync(dto.EId);
            return NoContent();
        }
    }
}
