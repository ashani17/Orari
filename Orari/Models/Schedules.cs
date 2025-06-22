using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Orari.Models
{
    public class Schedules
    {
        [Key]
        public int SId { get; set; }
        public DateOnly Date { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
        
        public int RId { get; set; }
        [ForeignKey("RId")]
        public required Rooms Room { get; set; }
        
        // Reference to User (professor) instead of old PId structure
        public string? ProfessorId { get; set; }
        [ForeignKey("ProfessorId")]
        public User? Professor { get; set; }
        
        public int CId { get; set; }
        [ForeignKey("CId")]
        public required Courses Course { get; set; }
        
        public int? EId { get; set; }
        [ForeignKey("EId")]
        public Exams? Exam { get; set; }
    }
}
