using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Orari.DTO.StudyProgramDTO
{
    public class GetDelStudyProgramDTO
    {
        [JsonPropertyName("spId")]
        public int SPId { get; set; }

        [JsonPropertyName("spName")]
        public string SPName { get; set; } = string.Empty;

        [JsonPropertyName("dId")]
        public int DId { get; set; } // Foreign key to Departments
        
        [JsonPropertyName("dName")]
        public string DName { get; set; } = string.Empty; // Navigation property
        
        [JsonPropertyName("courses")]
        public List<StudyProgramCourseInfo> Courses { get; set; } = new List<StudyProgramCourseInfo>();
    }

    public class StudyProgramCourseInfo
    {
        [JsonPropertyName("courseId")]
        public int CourseId { get; set; }
        
        [JsonPropertyName("courseName")]
        public string CourseName { get; set; } = string.Empty;
        
        [JsonPropertyName("credits")]
        public int Credits { get; set; }
        
        [JsonPropertyName("professor")]
        public string Professor { get; set; } = string.Empty;
        
        [JsonPropertyName("year")]
        public int Year { get; set; } // 1, 2, or 3
        
        [JsonPropertyName("academicYear")]
        public string AcademicYear { get; set; } = string.Empty; // e.g., "2023-2026"
    }
}
