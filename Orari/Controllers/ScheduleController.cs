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
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

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
                    Description = schedule.Description,
                    RId = schedule.RId,
                    ProfessorId = schedule.ProfessorId,
                    CId = schedule.CId,
                    Room = room,
                    Course = course,
                    EId = null,  // No exam initially
                    Exam = null
                };

                var createdSchedule = await _scheduleService.CreateScheduleAsync(scheduleModel);

                // If this is an exam, create an exam record
                if (schedule.IsExam && !string.IsNullOrEmpty(schedule.ExamName))
                {
                    var exam = new Exams
                    {
                        ExamName = schedule.ExamName,
                        ExamDate = schedule.Date.ToDateTime(TimeOnly.MinValue), // Convert DateOnly to DateTime
                        StartTime = schedule.StartTime.ToTimeSpan(), // Convert TimeOnly to TimeSpan
                        EndTime = schedule.EndTime.ToTimeSpan(), // Convert TimeOnly to TimeSpan
                        CId = schedule.CId,
                        ProfessorId = schedule.ProfessorId,
                        RId = schedule.RId,
                        SCId = createdSchedule.SId,
                        Course = course,
                        Room = room
                    };

                    var createdExam = await _examService.CreateExamAsync(exam);

                    // Update the schedule with the exam ID
                    createdSchedule.EId = createdExam.EId;
                    createdSchedule.Exam = createdExam;
                    await _scheduleService.UpdateScheduleAsync(createdSchedule);
                }

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

        [HttpGet("student/{studentId}")]
        [ProducesResponseType(typeof(IEnumerable<Schedules>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSchedulesByStudent(string studentId)
        {
            var schedules = await _scheduleService.GetSchedulesByStudentAsync(studentId);
            return Ok(schedules);
        }

        [HttpGet("dashboard-full")]
        public async Task<IActionResult> GetFullScheduleDashboard(
            [FromQuery] int? year = null,
            [FromQuery] DateTime? weekStart = null,
            [FromQuery] DateTime? weekEnd = null,
            [FromQuery] string? studyProgram = null,
            [FromQuery] string? professor = null,
            [FromQuery] string? course = null,
            [FromQuery] string? room = null,
            [FromQuery] string? academicYear = null,
            [FromQuery] string? group = null)
        {
            // Eager load all related data
            using (var scope = HttpContext.RequestServices.CreateScope())
            {
                var db = (Orari.DataDbContext.AppDbContext)scope.ServiceProvider.GetService(typeof(Orari.DataDbContext.AppDbContext));
                var schedulesQuery = db.Schedules
                    .Include(s => s.Room)
                    .Include(s => s.Professor)
                    .Include(s => s.Course)
                        .ThenInclude(c => c.StudyProgramCourse)
                            .ThenInclude(spc => spc.StudyProgram)
                                .ThenInclude(sp => sp.Departments)
                    .AsQueryable();

                // Apply filters
                if (year.HasValue)
                {
                    schedulesQuery = schedulesQuery.Where(s => s.Date.Year == year.Value);
                }

                if (!string.IsNullOrEmpty(studyProgram))
                {
                    schedulesQuery = schedulesQuery.Where(s => s.Course.StudyProgramCourse.Any(spc => 
                        spc.StudyProgram.SPName.ToLower().Contains(studyProgram.ToLower())));
                }

                if (!string.IsNullOrEmpty(professor))
                {
                    schedulesQuery = schedulesQuery.Where(s => 
                        (s.Professor.FirstName + " " + s.Professor.LastName).ToLower().Contains(professor.ToLower()));
                }

                if (!string.IsNullOrEmpty(course))
                {
                    schedulesQuery = schedulesQuery.Where(s => s.Course.CName.ToLower().Contains(course.ToLower()));
                }

                if (!string.IsNullOrEmpty(room))
                {
                    schedulesQuery = schedulesQuery.Where(s => s.Room.RName.ToLower().Contains(room.ToLower()));
                }

                if (!string.IsNullOrEmpty(academicYear))
                {
                    schedulesQuery = schedulesQuery.Where(s => s.Course.StudyProgramCourse.Any(spc => 
                        spc.AcademicYear == academicYear));
                }

                if (!string.IsNullOrEmpty(group))
                {
                    schedulesQuery = schedulesQuery.Where(s => s.Group != null && s.Group.ToLower().Contains(group.ToLower()));
                }

                // Add week filtering if provided
                if (weekStart.HasValue && weekEnd.HasValue)
                {
                    schedulesQuery = schedulesQuery.Where(s => s.Date >= DateOnly.FromDateTime(weekStart.Value) && s.Date <= DateOnly.FromDateTime(weekEnd.Value));
                }

                var schedules = await schedulesQuery.ToListAsync();

                var result = schedules.Select(s => {
                    // Get study program information from the first associated study program
                    var studyProgramCourse = s.Course?.StudyProgramCourse?.FirstOrDefault();
                    var studyProgram = studyProgramCourse?.StudyProgram;
                    var department = studyProgram?.Departments;
                    
                    return new Orari.DTO.ScheduleDTO.GetFullScheduleDTO
                    {
                        SId = s.SId,
                        Date = s.Date,
                        StartTime = s.StartTime,
                        EndTime = s.EndTime,
                        RId = s.RId,
                        RoomName = s.Room?.RName ?? string.Empty,
                        RoomType = s.Room?.RType ?? string.Empty,
                        RoomCapacity = s.Room?.RCapacity ?? 0,
                        RoomDescription = s.Room?.RDescription ?? string.Empty,
                        ProfessorId = s.ProfessorId,
                        ProfessorFirstName = s.Professor?.FirstName,
                        ProfessorLastName = s.Professor?.LastName,
                        ProfessorEmail = s.Professor?.Email,
                        CId = s.CId,
                        CourseName = s.Course?.CName ?? string.Empty,
                        Credits = s.Course?.Credits ?? 0,
                        StudyProgramId = studyProgram?.SPId,
                        StudyProgramName = studyProgram?.SPName,
                        DepartmentId = department?.DId,
                        DepartmentName = department?.DName,
                        Year = studyProgramCourse?.Year ?? 0,
                        AcademicYear = studyProgramCourse?.AcademicYear ?? string.Empty,
                        Group = s.Group // Add group to DTO
                    };
                }).ToList();

                return Ok(result);
            }
        }

        [AllowAnonymous]
        [HttpGet("rooms")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllRooms()
        {
            var rooms = await _roomService.GetAllRooms();
            return Ok(rooms);
        }
    }
}
