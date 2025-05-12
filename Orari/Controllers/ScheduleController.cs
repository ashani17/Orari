using Microsoft.AspNetCore.Mvc;
using Orari.DataDbContext;
using Orari.DTO.ExamDTO;
using Orari.DTO.ProfesorDTO;
using Orari.DTO.RoomDTO;
using Orari.DTO.ScheduleDTO;
using Orari.Interfaces;
using Orari.Models;
using Orari.Repository;
using Orari.Services;

namespace Orari.Controllers
{
    [Route("api/schedule")]
    public class ScheduleController : Controller
    {

        private readonly IScheduleService _scheduleService;
        private readonly IRoomService _roomService;
        private readonly IProfesorService _profesorService;
        private readonly ICourseService _courseService;
        private readonly IExamService _examService;

        public ScheduleController(IScheduleService scheduleService, IExamService examService, IRoomService roomService, IProfesorService profesorService, ICourseService courseService)
        {

            _scheduleService = scheduleService;
            _courseService = courseService;
            _examService = examService;
            _roomService = roomService;
            _profesorService = profesorService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var schedules = await _scheduleService.GetAllSchedules();
            return Ok(schedules);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromBody] GetDelScheduleDTO dto)
        {
            var schedule = await _scheduleService.GetScheduleByIdAsync(dto.SCId);
            if (schedule == null)
            {
                return NotFound();
            }
            return Ok(schedule);
        }

        [HttpPost]
        public async Task<IActionResult> CreateScheduleAsync([FromBody] PostScheduleDTO schedule)
        {
            try
            {
                if (schedule == null)
                {
                    return BadRequest("Schedule data is required");
                }

                // Map the PostScheduleDTO to the Schedules model
                var scheduleModel = new Schedules
                {
                    Date = schedule.Date,
                    StartTime = schedule.StartTime,
                    EndTime = schedule.EndTime,
                    RId = schedule.RId,
                    PId = schedule.PId,
                    CId = schedule.CId,
                    Room = schedule.Room,
                    Profesor = schedule.Profesor,
                    Course = schedule.Course,
                    EId = null,  // No exam initially
                    Exam = null
                };

                var createdSchedule = await _scheduleService.CreateScheduleAsync(scheduleModel);
                return Ok(createdSchedule);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] PutScheduleDTO schedule)
        {
            if (schedule == null)
            {
                return BadRequest();
            }
            var existingSchedule = await _scheduleService.GetScheduleByIdAsync(id);
            if (existingSchedule == null)
            {
                return NotFound();
            }
            existingSchedule.Date = schedule.Date;
            existingSchedule.StartTime = schedule.StartTime;
            existingSchedule.EndTime = schedule.EndTime;
            existingSchedule.Profesor = schedule.Profesor;
            existingSchedule.Room = schedule.Room;
            existingSchedule.Course = schedule.Course;
            existingSchedule.Exam = schedule.Exam;


            var updatedSchedule = await _scheduleService.UpdateScheduleAsync(existingSchedule);
            return Ok(updatedSchedule);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromBody] GetDelScheduleDTO dto)
        {
            var schedule = await _scheduleService.GetScheduleByIdAsync(dto.SCId);
            if (schedule == null)
            {
                return NotFound();
            }
            await _scheduleService.DeleteScheduleAsync(dto.SCId);
            return NoContent();
        }

        [HttpGet("profesor/{id}")]
        public async Task<IActionResult> GetByProfesor([FromBody] GetDelProfesorDTO dto)
        {
            var schedules = await _scheduleService.GetSchedulesByProfesorAsync(dto.PId);
            return Ok(schedules);
        }

        [HttpGet("room/{id}")]
        public async Task<IActionResult> GetByRoom([FromBody] GetDelRoomDTO dto)
        {
            var schedules = await _scheduleService.GetSchedulesByRoomAsync(dto.RId);
            return Ok(schedules);
        }

        [HttpPost("{id}/exam")]
        public async Task<IActionResult> AddExamToSchedule(int id, [FromBody] AddExamToScheduleDTO examDto)
        {
            try
            {
                var schedule = await _scheduleService.GetScheduleByIdAsync(id);
                if (schedule == null)
                {
                    return NotFound("Schedule not found");
                }

                // Create the exam
                var exam = new Exams
                {
                    ExamName = examDto.ExamName,
                    ExamDate = examDto.ExamDate,
                    StartTime = examDto.StartTime,
                    EndTime = examDto.EndTime,
                    CId = schedule.CId,
                    PId = schedule.PId,
                    RId = schedule.RId,
                    SCId = schedule.SCId
                };

                // Create the exam
                var createdExam = await _examService.CreateExamAsync(exam);

                // Update the schedule with the exam ID
                schedule.EId = createdExam.EId;
                schedule.Exam = createdExam;
                await _scheduleService.UpdateScheduleAsync(schedule);

                return Ok(schedule);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
