namespace Orari.Models
{
    public class Schedules
    {
        public int SCId { get; set; }
        public DateOnly Date { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
        public required int RId {  get; set; }
        public required string Room { get; set; }
        public required int PId { get; set; }
        public required string Profesor { get; set; }
        public required int CId { get; set; }
        public required string Course { get; set; }
        public required int EId { get; set; }
        public required Exams Exam { get; set; }
    }
}
