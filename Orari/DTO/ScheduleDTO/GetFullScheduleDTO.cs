namespace Orari.DTO.ScheduleDTO
{
    public class GetFullScheduleDTO
    {
        public int SId { get; set; }
        public DateOnly Date { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
        public string? Description { get; set; }
        public int RId { get; set; }
        public string RoomName { get; set; } = string.Empty;
        public string RoomType { get; set; } = string.Empty;
        public int RoomCapacity { get; set; }
        public string RoomDescription { get; set; } = string.Empty;
        public string? ProfessorId { get; set; }
        public string? ProfessorFirstName { get; set; }
        public string? ProfessorLastName { get; set; }
        public string? ProfessorEmail { get; set; }
        public int CId { get; set; }
        public string CourseName { get; set; } = string.Empty;
        public int Credits { get; set; }
    }
} 