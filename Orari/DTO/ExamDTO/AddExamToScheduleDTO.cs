namespace Orari.DTO.ExamDTO
{
    public class AddExamToScheduleDTO
    {
            public int ScheduleId { get; set; }
            public string ExamName { get; set; }
            public DateOnly ExamDate { get; set; }
            public TimeOnly StartTime { get; set; }
            public TimeOnly EndTime { get; set; }
    }
}
