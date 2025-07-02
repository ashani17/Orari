using Microsoft.AspNetCore.Mvc;
using Orari.DTO.StudyProgramDTO;
using Orari.Interfaces;
using Orari.Models;
using Microsoft.EntityFrameworkCore;
using Orari.DataDbContext;

namespace Orari.Controllers
{
    [ApiController]
    [Route("api/studyprogram")]
    public class StudyProgramController : Controller
    {
        private readonly IStudyProgramService _studyProgramService;
        public StudyProgramController(IStudyProgramService studyProgramService)
        {
            _studyProgramService = studyProgramService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllStudyPrograms()
        {
            var studyPrograms = await _studyProgramService.GetAllStudyProgramAsync();
            return Ok(studyPrograms);
        }
        
        [HttpGet("{id}")]
        public async Task<IActionResult> GetStudyProgramByIdAsync([FromRoute] int id)
        {
            var studyProgram = await _studyProgramService.GetStudyProgramByIdAsync(id);
            if (studyProgram == null)
            {
                return NotFound();
            }
            return Ok(studyProgram);
        }

        [HttpGet("{id}/with-courses")]
        public async Task<IActionResult> GetStudyProgramWithCoursesAsync([FromRoute] int id)
        {
            using (var scope = HttpContext.RequestServices.CreateScope())
            {
                var db = (Orari.DataDbContext.AppDbContext)scope.ServiceProvider.GetService(typeof(Orari.DataDbContext.AppDbContext));
                
                var studyProgram = await db.StudyPrograms
                    .Include(sp => sp.Departments)
                    .Include(sp => sp.StudyProgramCourse)
                        .ThenInclude(spc => spc.Course)
                    .FirstOrDefaultAsync(sp => sp.SPId == id);

                if (studyProgram == null)
                {
                    return NotFound();
                }

                var result = new GetDelStudyProgramDTO
                {
                    SPId = studyProgram.SPId,
                    SPName = studyProgram.SPName,
                    DId = studyProgram.DId,
                    DName = studyProgram.Departments?.DName ?? string.Empty,
                    Courses = studyProgram.StudyProgramCourse.Select(spc => new StudyProgramCourseInfo
                    {
                        CourseId = spc.CId,
                        CourseName = spc.Course?.CName ?? string.Empty,
                        Credits = spc.Course?.Credits ?? 0,
                        Professor = spc.Course?.Profesor ?? string.Empty,
                        Year = spc.Year,
                        AcademicYear = spc.AcademicYear
                    }).ToList()
                };

                return Ok(result);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateStudyProgramAsync([FromBody] PostStudyProgramDTO studyProgram)
        {
            if (studyProgram == null)
            {
                return BadRequest();
            }

            // Map the DTO to the StudyPrograms model
            var studyProgramModel = new StudyPrograms
            {
                SPName = studyProgram.SPName,
                DId = studyProgram.DId,
            };

            var createdStudyProgram = await _studyProgramService.CreateStudyProgramAsync(studyProgramModel);
            return CreatedAtAction(nameof(GetStudyProgramByIdAsync), new { id = createdStudyProgram.SPId }, createdStudyProgram);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateStudyProgramAsync([FromQuery] int id, [FromBody] PutStudyProgramDTO studyProgram)
        {
            var existingStudyProgram = await _studyProgramService.GetStudyProgramByIdAsync(id);
            if (existingStudyProgram == null)
            {
                return NotFound();
            }

            // Update the existing entity properties
            existingStudyProgram.SPName = studyProgram.SPName;
            existingStudyProgram.DId = studyProgram.DId;

            var updatedStudyProgram = await _studyProgramService.UpdateStudyProgramAsync(existingStudyProgram);

            // --- Update StudyProgramCourse join table with year and academic year ---
            using (var scope = HttpContext.RequestServices.CreateScope())
            {
                var db = (Orari.DataDbContext.AppDbContext)scope.ServiceProvider.GetService(typeof(Orari.DataDbContext.AppDbContext));
                var oldLinks = db.StudyProgramCourses.Where(spc => spc.SPId == id);
                db.StudyProgramCourses.RemoveRange(oldLinks);

                if (studyProgram.CourseAssignments != null)
                {
                    foreach (var assignment in studyProgram.CourseAssignments)
                    {
                        db.StudyProgramCourses.Add(new Orari.Models.StudyProgramCourse
                        {
                            SPId = id,
                            CId = assignment.CourseId,
                            Year = assignment.Year,
                            AcademicYear = assignment.AcademicYear
                        });
                    }
                }
                db.SaveChanges();
            }
            // --- END ---

            return Ok(updatedStudyProgram);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudyProgramAsync([FromRoute] int id)
        {
            var existingStudyProgram = await _studyProgramService.GetStudyProgramByIdAsync(id);
            if (existingStudyProgram == null)
            {
                return NotFound();
            }
            await _studyProgramService.DeleteStudyProgramAsync(id);
            return NoContent();
        }

        [HttpGet("name/{name}")]
        public async Task<IActionResult> GetStudyProgramByNameAsync([FromRoute] string name)
        {
            var studyProgram = await _studyProgramService.GetStudyProgramsByNameAsync(name);
            if (studyProgram == null)
            {
                return NotFound();
            }
            return Ok(studyProgram);
        }

        [HttpGet("{id}/schedules")]
        public async Task<IActionResult> GetSchedulesByStudyProgram(int id, [FromQuery] int? year = null, [FromQuery] string? academicYear = null)
        {
            using (var scope = HttpContext.RequestServices.CreateScope())
            {
                var db = (Orari.DataDbContext.AppDbContext)scope.ServiceProvider.GetService(typeof(Orari.DataDbContext.AppDbContext));
                
                // Get StudyProgramCourses for this study program
                var spcQuery = db.StudyProgramCourses.Where(spc => spc.SPId == id);
                if (year.HasValue)
                {
                    spcQuery = spcQuery.Where(spc => spc.Year == year.Value);
                }
                if (!string.IsNullOrEmpty(academicYear))
                {
                    spcQuery = spcQuery.Where(spc => spc.AcademicYear == academicYear);
                }
                var spcList = await spcQuery.ToListAsync();
                var courseIds = spcList.Select(spc => spc.CId).ToList();

                if (!courseIds.Any())
                {
                    return Ok(new List<object>()); // Return empty list if no courses found
                }

                // Join schedules with StudyProgramCourses to include year and academicYear
                var scheduleResults = await (
                    from s in db.Schedules
                    join spc in db.StudyProgramCourses on new { s.CId, SPId = id } equals new { CId = spc.CId, spc.SPId }
                    where courseIds.Contains(s.CId)
                    select new
                    {
                        s.SId,
                        s.Date,
                        s.StartTime,
                        s.EndTime,
                        s.RId,
                        RoomName = s.Room.RName,
                        s.ProfessorId,
                        ProfessorFirstName = s.Professor.FirstName,
                        ProfessorLastName = s.Professor.LastName,
                        s.CId,
                        CourseName = s.Course.CName,
                        spc.Year,
                        spc.AcademicYear
                    }
                ).OrderBy(x => x.Date).ThenBy(x => x.StartTime).ToListAsync();

                return Ok(scheduleResults);
            }
        }
    }
}
