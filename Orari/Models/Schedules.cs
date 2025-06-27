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
        
        public string? Description { get; set; }  // Optional description
        
        public int RId { get; set; }
        [ForeignKey("RId")]
        public Rooms Room { get; set; } = null!;
        
        // Reference to User (professor) instead of old PId structure
        public string? ProfessorId { get; set; }
        [ForeignKey("ProfessorId")]
        public User? Professor { get; set; }
        
        public int CId { get; set; }
        [ForeignKey("CId")]
        public Courses Course { get; set; } = null!;
        
        public int? EId { get; set; }
        [ForeignKey("EId")]
        public Exams? Exam { get; set; }
        
        public int? RecurringScheduleId { get; set; }
        [ForeignKey("RecurringScheduleId")]
        public RecurringSchedule? RecurringSchedule { get; set; }
    }
}
