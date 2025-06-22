using System.ComponentModel.DataAnnotations;

namespace Orari.DTO.EnrollmentDTO
{
    public class EnrollmentDto
    {
        public int EId { get; set; }
        public string StudentId { get; set; } = string.Empty;
        public int CId { get; set; }
        // Add other simple fields as needed, but do NOT include navigation properties like Student or Courses
    }

    public class EnrollmentWithDetailsDTO
    {
        public int EId { get; set; }
        public string StudentId { get; set; } = string.Empty;
        public string StudentName { get; set; } = string.Empty;
        public string StudentEmail { get; set; } = string.Empty;
        public int CId { get; set; }
        public string CourseName { get; set; } = string.Empty;
        public int CourseCredits { get; set; }
        public string ProfessorName { get; set; } = string.Empty;
    }

    public class EnrollmentSummaryDTO
    {
        public int EId { get; set; }
        public string StudentId { get; set; } = string.Empty;
        public string StudentName { get; set; } = string.Empty;
        public string StudentEmail { get; set; } = string.Empty;
        public int CId { get; set; }
        public string CourseName { get; set; } = string.Empty;
    }
}
