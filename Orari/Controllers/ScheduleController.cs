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
        private readonly IExamService _examService;
        private readonly IRoomService _roomService;
        private readonly ICourseService _courseService;

        public ScheduleController(IScheduleService scheduleService, IExamService examService, IRoomService roomService, ICourseService courseService)
        {
            _scheduleService = scheduleService;
            _examService = examService;
            _roomService = roomService;
            _courseService = courseService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Schedules>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllSchedules()
        {
            var schedules = await _scheduleService.GetAllSchedulesAsync();
            return Ok(schedules);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Schedules), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetScheduleById(int id)
        {
            var schedule = await _scheduleService.GetScheduleByIdAsync(id);
            if (schedule == null)
            {
                return NotFound("Schedule not found");
            }
            return Ok(schedule);
        }

        [HttpPost]
        [ProducesResponseType(typeof(Schedules), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateSchedule([FromBody] PostScheduleDTO schedule)
        {
            try
            {
                if (schedule == null)
                {
                    return BadRequest("Schedule data is required");
                }

                // Get the required entities
                var room = await _roomService.GetRoomByIdAsync(schedule.RId);
                var course = await _courseService.GetCourseByIdAsync(schedule.CId);
                
                if (room == null)
                {
                    return BadRequest("Room not found");
                }
                
                if (course == null)
                {
                    return BadRequest("Course not found");
                }

                // Map the PostScheduleDTO to the Schedules model
                var scheduleModel = new Schedules
                {
                    Date = schedule.Date,
                    StartTime = schedule.StartTime,
                    EndTime = schedule.EndTime,
                    RId = schedule.RId,
                    ProfessorId = schedule.ProfessorId,
                    CId = schedule.CId,
                    Room = room,
                    Course = course,
                    EId = null,  // No exam initially
                    Exam = null
                };

                var createdSchedule = await _scheduleService.CreateScheduleAsync(scheduleModel);
                return CreatedAtAction(nameof(GetScheduleById), new { id = createdSchedule.SId }, createdSchedule);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(Schedules), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateSchedule(int id, [FromBody] PutScheduleDTO schedule)
        {
            try
            {
                if (schedule == null)
                {
                    return BadRequest("Schedule data is required");
                }
                
                var existingSchedule = await _scheduleService.GetScheduleByIdAsync(id);
                if (existingSchedule == null)
                {
                    return NotFound("Schedule not found");
                }
                
                existingSchedule.Date = schedule.Date;
                existingSchedule.StartTime = schedule.StartTime;
                existingSchedule.EndTime = schedule.EndTime;
                existingSchedule.ProfessorId = schedule.ProfessorId;
                existingSchedule.Exam = schedule.Exam;

                var updatedSchedule = await _scheduleService.UpdateScheduleAsync(existingSchedule);
                return Ok(updatedSchedule);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteSchedule(int id)
        {
            var schedule = await _scheduleService.GetScheduleByIdAsync(id);
            if (schedule == null)
            {
                return NotFound("Schedule not found");
            }
            await _scheduleService.DeleteScheduleAsync(id);
            return NoContent();
        }

        [HttpGet("professor/{professorId}")]
        [ProducesResponseType(typeof(IEnumerable<Schedules>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetSchedulesByProfessor(string professorId)
        {
            try
            {
                var schedules = await _scheduleService.GetSchedulesByProfessorAsync(professorId);
                return Ok(schedules);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("room/{roomId}")]
        [ProducesResponseType(typeof(IEnumerable<Schedules>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetSchedulesByRoom(int roomId)
        {
            try
            {
                // Since we removed room-specific functionality, return empty list for now
                // This endpoint can be removed or updated based on requirements
                return Ok(new List<Schedules>());
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost("{id}/exam")]
        [ProducesResponseType(typeof(Schedules), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddExamToSchedule(int id, [FromBody] AddExamToScheduleDTO examDto)
        {
            try
            {
                var schedule = await _scheduleService.GetScheduleByIdAsync(id);
                if (schedule == null)
                {
                    return NotFound("Schedule not found");
                }

                // Get the required Course and Room for the exam
                var course = await _courseService.GetCourseByIdAsync(schedule.CId);
                var room = await _roomService.GetRoomByIdAsync(schedule.RId);
                
                if (course == null)
                {
                    return BadRequest("Course not found");
                }
                
                if (room == null)
                {
                    return BadRequest("Room not found");
                }

                // Create the exam with required members
                var exam = new Exams
                {
                    ExamName = examDto.ExamName,
                    ExamDate = examDto.ExamDate.ToDateTime(TimeOnly.MinValue), // Convert DateOnly to DateTime
                    StartTime = examDto.StartTime.ToTimeSpan(), // Convert TimeOnly to TimeSpan
                    EndTime = examDto.EndTime.ToTimeSpan(), // Convert TimeOnly to TimeSpan
                    CId = schedule.CId,
                    ProfessorId = schedule.ProfessorId, // Use ProfessorId instead of PId
                    RId = schedule.RId,
                    SCId = schedule.SId, // Use SId instead of SCId
                    Course = course, // Required member
                    Room = room // Required member
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
