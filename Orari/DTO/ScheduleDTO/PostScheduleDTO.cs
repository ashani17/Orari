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
        public string Room { get; set; }
        [Required]
        public string Profesor { get; set; }
        [Required]
        public string Course { get; set; }
    }
}
