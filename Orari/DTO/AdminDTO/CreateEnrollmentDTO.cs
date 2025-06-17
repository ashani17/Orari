using System.ComponentModel.DataAnnotations;

namespace Orari.DTO.AdminDTO
{
    public class CreateEnrollmentDTO
    {
        [Required]
        public string StudentId { get; set; } = string.Empty;

        [Required]
        public int CourseId { get; set; }
    }
} 