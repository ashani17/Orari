using Microsoft.AspNetCore.Mvc;
using Orari.DataDbContext;
using Orari.Interfaces;
using Orari.Models;

namespace Orari.Controllers
{
    [Route("api/schedule")]
    public class ScheduleController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IScheduleRepository _scheduleRepository;

        public ScheduleController(AppDbContext context, IScheduleRepository scheduleRepository)
        {
            _context = context;
            _scheduleRepository = scheduleRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var schedules = await _scheduleRepository.GetAllSchedules();
            return Ok(schedules);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var schedule = await _scheduleRepository.GetScheduleByIdAsync(id);
            if (schedule == null)
            {
                return NotFound();
            }
            return Ok(schedule);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Schedules schedule)
        {
            if (schedule == null)
            {
                return BadRequest();
            }
            var createdSchedule = await _scheduleRepository.CreateScheduleAsync(schedule);
            return CreatedAtAction(nameof(GetById), new { id = createdSchedule.SCId }, createdSchedule);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Schedules schedule)
        {
            if (schedule == null)
            {
                return BadRequest();
            }
            var existingSchedule = await _scheduleRepository.GetScheduleByIdAsync(id);
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
            var updatedSchedule = await _scheduleRepository.UpdateScheduleAsync(existingSchedule);
            return Ok(updatedSchedule);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var schedule = await _scheduleRepository.GetScheduleByIdAsync(id);
            if (schedule == null)
            {
                return NotFound();
            }
            await _scheduleRepository.DeleteScheduleAsync(id);
            return NoContent();
        }

        [HttpGet("profesor/{id}")]
        public async Task<IActionResult> GetByProfesor(int id)
        {
            var schedules = await _scheduleRepository.GetSchedulesByProfesorAsync(id);
            return Ok(schedules);
        }

        [HttpGet("room/{id}")]
        public async Task<IActionResult> GetByRoom(int id)
        {
            var schedules = await _scheduleRepository.GetSchedulesByRoomAsync(id);
            return Ok(schedules);
        }

    }
}
