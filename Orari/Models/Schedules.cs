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
        public int RId {  get; set; }
        public required string Room { get; set; }
        [ForeignKey("PId")]
        public int PId { get; set; }
        public required string Profesor { get; set; }
        [ForeignKey("CId")]
        public int CId { get; set; }
        public required string Course { get; set; }
        [ForeignKey("EId")]
        public int EId { get; set; }
        public Exams Exam { get; set; }
    }
}
