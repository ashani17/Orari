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
        public int PId { get; set; }  // Professor ID

        [Required]
        public int CId { get; set; }  // Course ID

        [Required]
        public string Room { get; set; }

        [Required]
        public string Profesor { get; set; }

        [Required]
        public string Course { get; set; }
    }
}
