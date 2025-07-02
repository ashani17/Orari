using System.Text.Json.Serialization;

namespace Orari.DTO.StudyProgramDTO
{
    public class PutStudyProgramDTO
    {
        [JsonPropertyName("spName")]
        public string SPName { get; set; } = string.Empty;
        
        [JsonPropertyName("dId")]
        public int DId { get; set; } // Foreign key to Departments
        
        [JsonPropertyName("courseAssignments")]
        public List<StudyProgramCourseAssignment> CourseAssignments { get; set; } = new List<StudyProgramCourseAssignment>();
    }

    public class StudyProgramCourseAssignment
    {
        [JsonPropertyName("courseId")]
        public int CourseId { get; set; }
        
        [JsonPropertyName("year")]
        public int Year { get; set; } // 1, 2, or 3
        
        [JsonPropertyName("academicYear")]
        public string AcademicYear { get; set; } = string.Empty; // e.g., "2023-2026"
    }
}
