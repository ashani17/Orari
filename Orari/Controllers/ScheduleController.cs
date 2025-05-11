using Microsoft.AspNetCore.Mvc;
using Orari.DataDbContext;
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
        public async Task<IActionResult> GetById(int id)
        {
            var schedule = await _scheduleService.GetScheduleByIdAsync(id);
            if (schedule == null)
            {
                return NotFound();
            }
            return Ok(schedule);
        }

        [HttpPost]
        public async Task<IActionResult> CreateScheduleAsync([FromBody] PostScheduleDTO schedule)
        {
            if (schedule == null)
            {
                return BadRequest();
            }
            // Map the PostScheduleDTO to the Schedules model
            var scheduleModel = new Schedules
            {
                Date = schedule.Date,
                StartTime = schedule.StartTime,
                EndTime = schedule.EndTime,
                Room = schedule.Room,
                Profesor = schedule.Profesor,
                Course = schedule.Course
            };

            // Pass the mapped Schedules object to the service
            var createdSchedule = await _scheduleService.CreateScheduleAsync(scheduleModel);
            return Ok(createdSchedule);
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
        public async Task<IActionResult> Delete(int id)
        {
            var schedule = await _scheduleService.GetScheduleByIdAsync(id);
            if (schedule == null)
            {
                return NotFound();
            }
            await _scheduleService.DeleteScheduleAsync(id);
            return NoContent();
        }

        [HttpGet("profesor/{id}")]
        public async Task<IActionResult> GetByProfesor(int id)
        {
            var schedules = await _scheduleService.GetSchedulesByProfesorAsync(id);
            return Ok(schedules);
        }

        [HttpGet("room/{id}")]
        public async Task<IActionResult> GetByRoom(int id)
        {
            var schedules = await _scheduleService.GetSchedulesByRoomAsync(id);
            return Ok(schedules);
        }

    }
}
