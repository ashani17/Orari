using Orari.DTO.ExamDTO;
using Orari.Models;
using System.ComponentModel.DataAnnotations;

namespace Orari.DTO.ScheduleDTO
{
    public class PutScheduleDTO
    {
        [Required]
        public DateOnly Date { get; set; }
        
        [Required]
        public TimeOnly StartTime { get; set; }
        
        [Required]
        public TimeOnly EndTime { get; set; }
        
        [Required]
        public string ProfessorId { get; set; }  // Professor User ID (string)
        
        public Exams? Exam { get; set; }
    }
}
