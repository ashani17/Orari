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
        public int? StudyProgramId { get; set; }
        public string? StudyProgramName { get; set; }
        public int? DepartmentId { get; set; }
        public string? DepartmentName { get; set; }
        public int Year { get; set; } // 1, 2, or 3
        public string AcademicYear { get; set; } = string.Empty; // e.g., "2023-2026"
        public string? Group { get; set; } // e.g., A1, A2, B1, etc.
    }
} 