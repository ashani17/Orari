using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Orari.Models
{
    public class Exams
    {
        [Key]
        public int EId { get; set; }
        public required string ExamName { get; set; }
        public required DateOnly ExamDate { get; set; }
        public required TimeOnly StartTime { get; set; }
        public required TimeOnly EndTime { get; set; }
        [ForeignKey("CId")]
        public int CId { get; set; }
        public Courses Course { get; set; }
        [ForeignKey("SCId")]
        public int SCId { get; set; }
        public Schedules Schedule { get; set; }

        [ForeignKey("PId")]
        public int PId { get; set; }
        public Profesors Profesor { get; set; }

        [ForeignKey("RId")]
        public int RId { get; set; }
        public Rooms Room { get; set; }

    }
}
