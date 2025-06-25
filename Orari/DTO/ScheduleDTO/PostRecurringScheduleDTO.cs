namespace Orari.DTO.ScheduleDTO
{
    public class PostRecurringScheduleDTO
    {
        public int CourseId { get; set; }
        public int RoomId { get; set; }
        public string ProfessorId { get; set; } = string.Empty;
        public int DayOfWeek { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
    }
} 