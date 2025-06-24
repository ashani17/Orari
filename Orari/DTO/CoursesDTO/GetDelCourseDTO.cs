using Orari.DTO.EnrollmentDTO;

namespace Orari.DTO.CoursesDTO
{
    public class GetDelCourseDTO
    {
        public int CId { get; set; }
        public string CName { get; set; } = string.Empty;
        public int Credits { get; set; }
        public string PId { get; set; } = string.Empty;
        public string Profesor { get; set; } = string.Empty;
        public int? StudyProgramId { get; set; }
        public string? StudyProgramName { get; set; }
    }

    public class CourseWithEnrollmentsDTO
    {
        public int CId { get; set; }
        public string CName { get; set; } = string.Empty;
        public int Credits { get; set; }
        public string PId { get; set; } = string.Empty;
        public string Profesor { get; set; } = string.Empty;
        public List<EnrollmentSummaryDTO> Enrollments { get; set; } = new List<EnrollmentSummaryDTO>();
    }
}
