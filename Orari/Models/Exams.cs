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

        public int CId { get; set; }
        public int CourseCId { get; set; }
        public Courses Course { get; set; }

        public int SCId { get; set; }
        public Schedules Schedule { get; set; }

        public int PId { get; set; }
        public int ProfesorPId { get; set; }
        public Profesors Profesor { get; set; }

        public int RId { get; set; }
        public int RoomRId { get; set; }
        public Rooms Room { get; set; }
    }
}
