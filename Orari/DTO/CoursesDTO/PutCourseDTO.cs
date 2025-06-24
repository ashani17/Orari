using Orari.Models;

namespace Orari.DTO.CoursesDTO
{
    public class PutCourseDTO
    {
        public int CId { get; set; }
        public required string CName { get; set; }
        public int Credits { get; set; }
        public string? ProfessorId { get; set; } // Reference to User (professor) instead of Profesors
        public string? ProfessorName { get; set; } // Professor name for display purposes
        public int StudyProgramId { get; set; }
    }
}
