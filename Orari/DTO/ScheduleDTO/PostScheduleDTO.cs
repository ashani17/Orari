using Orari.DTO.ExamDTO;
using Orari.Models;
using System.ComponentModel.DataAnnotations;

namespace Orari.DTO.ScheduleDTO
{
    public class PostScheduleDTO
    {
        [Required]
        public DateOnly Date { get; set; }

        [Required]
        public TimeOnly StartTime { get; set; }

        [Required]
        public TimeOnly EndTime { get; set; }

        [Required]
        public int RId { get; set; }  // Room ID

        [Required]
        public string ProfessorId { get; set; }  // Professor User ID (string)

        [Required]
        public int CId { get; set; }  // Course ID
    }
}
