using System.ComponentModel.DataAnnotations;
using Orari.DTO.ProfesorDTO;

namespace Orari.DTO.ScheduleDTO
{
    public class GetDelScheduleDTO
    {
        public int SId { get; set; }
        public DateOnly Date { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
        public string? Description { get; set; }
        public int RId { get; set; }
        public string? ProfessorId { get; set; }
        public int CId { get; set; }
        public int? EId { get; set; }
        public int? RecurringScheduleId { get; set; }
        
        // Include basic info without navigation properties
        public string? RoomName { get; set; }
        public string? CourseName { get; set; }
        public string? ProfessorName { get; set; }
    }
}
