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
}
