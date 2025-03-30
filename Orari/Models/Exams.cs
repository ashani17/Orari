using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Orari.Models
{
    public class Exams
    {
        [Key]
        public int EId { get; set; }
        public required string ExamName { get; set; }
        [ForeignKey("CId")]
        public required int CId { get; set; }
        public DateOnly ExamDate { get; set; }
        public required Courses Course { get; set; }
        [ForeignKey("SCId")]
        public required int SCId { get; set; }
        public required Schedules Schedule { get; set; }

        [ForeignKey("PId")]
        public required int PId { get; set; }
        public required Profesors Profesor { get; set; }

    }
}
