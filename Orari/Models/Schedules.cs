using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Orari.Models
{
    public class Schedules
    {
        [Key]
        public int SCId { get; set; }
        public DateOnly Date { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
        [ForeignKey("RId")]
        public required int RId {  get; set; }
        public required string Room { get; set; }
        [ForeignKey("PId")]
        public required int PId { get; set; }
        public required string Profesor { get; set; }
        [ForeignKey("CId")]
        public required int CId { get; set; }
        public required string Course { get; set; }
        [ForeignKey("EId")]
        public required int EId { get; set; }
        public required Exams Exam { get; set; }
    }
}
