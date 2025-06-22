using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Orari.Models
{
    public class Exams
    {
        [Key]
        public int EId { get; set; }

        [Required]
        public string ExamName { get; set; } = string.Empty;

        [Required]
        public DateTime ExamDate { get; set; }

        [Required]
        public TimeSpan StartTime { get; set; }

        [Required]
        public TimeSpan EndTime { get; set; }

        public int CId { get; set; }
        [ForeignKey("CId")]
        public required Courses Course { get; set; }

        public int? SCId { get; set; }
        [ForeignKey("SCId")]
        public Schedules? Schedule { get; set; }

        // Reference to User (professor) instead of Profesors
        public string? ProfessorId { get; set; }
        [ForeignKey("ProfessorId")]
        public User? Professor { get; set; }

        public int RId { get; set; }
        [ForeignKey("RId")]
        public required Rooms Room { get; set; }
    }
}
