namespace Orari.DTO.StudyProgramDTO
{
    public class PutStudyProgramDTO
    {
        public string SPName { get; set; } = string.Empty;
        public int DId { get; set; } // Foreign key to Departments
        public List<StudyProgramCourseAssignment> CourseAssignments { get; set; } = new List<StudyProgramCourseAssignment>();
    }

    public class StudyProgramCourseAssignment
    {
        public int CourseId { get; set; }
        public int Year { get; set; } // 1, 2, or 3
        public string AcademicYear { get; set; } = string.Empty; // e.g., "2023-2026"
    }
}
