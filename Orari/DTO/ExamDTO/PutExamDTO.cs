namespace Orari.DTO.ExamDTO
{
    public class PutExamDTO
    {
        public PutExamDTO()
        {
        }
        public PutExamDTO(string examName, int cId, DateOnly examDate, int scId, int pId, TimeOnly startTime, TimeOnly endTime)
        {
            ExamName = examName;
            CourseId = cId;
            ExamDate = examDate;
            StartTime = startTime;
            EndTime = endTime;
            ScheduleId = scId;
            ProfesorId = pId;
        }
        public string ExamName { get; set; } = string.Empty;
        public int CourseId { get; set; }
        public DateOnly ExamDate { get; set; } = DateOnly.FromDateTime(DateTime.Now);
        public TimeOnly StartTime { get; set; } = TimeOnly.FromDateTime(DateTime.Now);
        public TimeOnly EndTime { get; set; } = TimeOnly.FromDateTime(DateTime.Now);
        public int ScheduleId { get; set; }
        public int ProfesorId { get; set; }
        public int RoomId { get; set; }
    }
}
