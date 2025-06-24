using Microsoft.AspNetCore.Mvc;
using Orari.DTO.StudyProgramDTO;
using Orari.Interfaces;
using Orari.Models;

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
        public async Task<IActionResult> GetStudyProgramByIdAsync([FromBody] GetDelStudyProgramDTO dto)
        {
            var studyProgram = await _studyProgramService.GetStudyProgramByIdAsync(dto.SPId);
            if (studyProgram == null)
            {
                return NotFound();
            }
            return Ok(studyProgram);
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

            // --- Update StudyProgramCourse join table ---
            using (var scope = HttpContext.RequestServices.CreateScope())
            {
                var db = (Orari.DataDbContext.AppDbContext)scope.ServiceProvider.GetService(typeof(Orari.DataDbContext.AppDbContext));
                var oldLinks = db.StudyProgramCourses.Where(spc => spc.SPId == id);
                db.StudyProgramCourses.RemoveRange(oldLinks);

                if (studyProgram.CourseIds != null)
                {
                    foreach (var courseId in studyProgram.CourseIds)
                    {
                        db.StudyProgramCourses.Add(new Orari.Models.StudyProgramCourse
                        {
                            SPId = id,
                            CId = courseId
                        });
                    }
                }
                db.SaveChanges();
            }
            // --- END ---

            return Ok(updatedStudyProgram);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudyProgramAsync([FromBody] GetDelStudyProgramDTO dto)
        {
            var existingStudyProgram = await _studyProgramService.GetStudyProgramByIdAsync(dto.SPId);
            if (existingStudyProgram == null)
            {
                return NotFound();
            }
            await _studyProgramService.DeleteStudyProgramAsync(dto.SPId);
            return NoContent();
        }

        [HttpGet("name/{name}")]
        public async Task<IActionResult> GetStudyProgramByNameAsync([FromBody] GetDelStudyProgramDTO dto)
        {
            var studyProgram = await _studyProgramService.GetStudyProgramsByNameAsync(dto.SPName);
            if (studyProgram == null)
            {
                return NotFound();
            }
            return Ok(studyProgram);
        }

    }

}
