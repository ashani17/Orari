using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Orari.DataDbContext;
using Orari.Models;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Orari.DTO.ScheduleDTO;

namespace Orari.Controllers
{
    [ApiController]
    [Route("api/recurring-schedule")]
    [Authorize(Roles = "Admin")]
    public class RecurringScheduleController : ControllerBase
    {
        private readonly AppDbContext _context;
        public RecurringScheduleController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/recurring-schedule
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RecurringSchedule>>> GetAll()
        {
            return await _context.RecurringSchedules.ToListAsync();
        }

        // GET: api/recurring-schedule/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<RecurringSchedule>> Get(int id)
        {
            var schedule = await _context.RecurringSchedules.FindAsync(id);
            if (schedule == null) return NotFound();
            return schedule;
        }

        // POST: api/recurring-schedule
        [HttpPost]
        public async Task<ActionResult<RecurringSchedule>> Create([FromBody] PostRecurringScheduleDTO dto)
        {
            var schedule = new RecurringSchedule
            {
                CourseId = dto.CourseId,
                RoomId = dto.RoomId,
                ProfessorId = dto.ProfessorId,
                DayOfWeek = (System.DayOfWeek)dto.DayOfWeek,
                StartTime = dto.StartTime,
                EndTime = dto.EndTime,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate
            };
            _context.RecurringSchedules.Add(schedule);
            await _context.SaveChangesAsync();

            // Generate Schedules for each matching date in the range, skipping exceptions
            var exceptions = await _context.ScheduleExceptions
                .Where(e => e.RecurringScheduleId == schedule.Id && e.IsCancelled)
                .Select(e => e.Date)
                .ToListAsync();
            var schedulesToAdd = new List<Schedules>();
            for (var date = schedule.StartDate; date <= schedule.EndDate; date = date.AddDays(1))
            {
                if (date.DayOfWeek == schedule.DayOfWeek && !exceptions.Contains(date))
                {
                    schedulesToAdd.Add(new Schedules
                    {
                        Date = date,
                        StartTime = schedule.StartTime,
                        EndTime = schedule.EndTime,
                        RId = schedule.RoomId,
                        ProfessorId = schedule.ProfessorId,
                        CId = schedule.CourseId,
                        RecurringScheduleId = schedule.Id
                    });
                }
            }
            if (schedulesToAdd.Count > 0)
            {
                _context.Schedules.AddRange(schedulesToAdd);
                await _context.SaveChangesAsync();
            }

            return CreatedAtAction(nameof(Get), new { id = schedule.Id }, schedule);
        }

        // PUT: api/recurring-schedule/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] RecurringSchedule schedule)
        {
            if (id != schedule.Id) return BadRequest();
            _context.Entry(schedule).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/recurring-schedule/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var schedule = await _context.RecurringSchedules.FindAsync(id);
            if (schedule == null) return NotFound();

            // Delete all generated schedules by RecurringScheduleId
            var generatedSchedules = await _context.Schedules
                .Where(s => s.RecurringScheduleId == schedule.Id)
                .ToListAsync();

            if (generatedSchedules.Any())
            {
                _context.Schedules.RemoveRange(generatedSchedules);
            }

            _context.RecurringSchedules.Remove(schedule);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // --- ScheduleException endpoints ---

        // GET: api/recurring-schedule/{recurringScheduleId}/exceptions
        [HttpGet("{recurringScheduleId}/exceptions")]
        public async Task<ActionResult<IEnumerable<ScheduleException>>> GetExceptions(int recurringScheduleId)
        {
            return await _context.ScheduleExceptions
                .Where(e => e.RecurringScheduleId == recurringScheduleId)
                .ToListAsync();
        }

        // POST: api/recurring-schedule/{recurringScheduleId}/exceptions
        [HttpPost("{recurringScheduleId}/exceptions")]
        public async Task<ActionResult<ScheduleException>> CreateException(int recurringScheduleId, [FromBody] ScheduleException exception)
        {
            exception.RecurringScheduleId = recurringScheduleId;
            _context.ScheduleExceptions.Add(exception);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetExceptions), new { recurringScheduleId }, exception);
        }

        // PUT: api/recurring-schedule/exceptions/{id}
        [HttpPut("exceptions/{id}")]
        public async Task<IActionResult> UpdateException(int id, [FromBody] ScheduleException exception)
        {
            if (id != exception.Id) return BadRequest();
            _context.Entry(exception).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/recurring-schedule/exceptions/{id}
        [HttpDelete("exceptions/{id}")]
        public async Task<IActionResult> DeleteException(int id)
        {
            var exception = await _context.ScheduleExceptions.FindAsync(id);
            if (exception == null) return NotFound();
            _context.ScheduleExceptions.Remove(exception);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
} 