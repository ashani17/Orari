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
        public async Task<IActionResult> CreateStudyProgramAsync([FromBody] StudyPrograms studyProgram)
        {
            if (studyProgram == null)
            {
                return BadRequest();
            }
            var createdStudyProgram = await _studyProgramService.CreateStudyProgramAsync(studyProgram);
            return CreatedAtAction(nameof(GetStudyProgramByIdAsync), new { id = createdStudyProgram.SPId }, createdStudyProgram);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateStudyProgramAsync(int id, [FromBody] StudyPrograms studyProgram)
        {
            var existingStudyProgram = await _studyProgramService.GetStudyProgramsByNameAsync(studyProgram.SPName);
            if (existingStudyProgram == null)
            {
                return NotFound();
            }

            // Use the existing study program's ID to update the record
            studyProgram.SPId = existingStudyProgram.SPId;

            var updatedStudyProgram = await _studyProgramService.UpdateStudyProgramAsync(studyProgram);
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
