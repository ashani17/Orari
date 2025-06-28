using System.ComponentModel.DataAnnotations;

namespace Orari.DTO.StudyProgramDTO
{
    public class GetDelStudyProgramDTO
    {
        public int SPId { get; set; }

        public string SPName { get; set; } = string.Empty;

        public int DId { get; set; } // Foreign key to Departments
        public string DName { get; set; } = string.Empty; // Navigation property
        public List<StudyProgramCourseInfo> Courses { get; set; } = new List<StudyProgramCourseInfo>();
    }

    public class StudyProgramCourseInfo
    {
        public int CourseId { get; set; }
        public string CourseName { get; set; } = string.Empty;
        public int Credits { get; set; }
        public string Professor { get; set; } = string.Empty;
        public int Year { get; set; } // 1, 2, or 3
        public string AcademicYear { get; set; } = string.Empty; // e.g., "2023-2026"
    }
}
