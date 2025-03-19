namespace Orari.Models
{
    public class Exams
    {
        public int EId { get; set; }
        public required string ExamName { get; set; }
        public required int CId { get; set; }
        public DateOnly ExamDate { get; set; }
        public required Courses Course { get; set; }
        public required int SCId { get; set; }
        public required Schedules Schedule { get; set; }
    }
}
