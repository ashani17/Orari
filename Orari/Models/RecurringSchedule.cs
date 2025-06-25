using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Orari.Models
{
    public class RecurringSchedule
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int CourseId { get; set; }
        [ForeignKey("CourseId")]
        public Courses Course { get; set; } = null!;

        [Required]
        public int RoomId { get; set; }
        [ForeignKey("RoomId")]
        public Rooms Room { get; set; } = null!;

        [Required]
        public string ProfessorId { get; set; } = string.Empty;
        [ForeignKey("ProfessorId")]
        public User Professor { get; set; } = null!;

        [Required]
        public DayOfWeek DayOfWeek { get; set; }

        [Required]
        public TimeOnly StartTime { get; set; }
        [Required]
        public TimeOnly EndTime { get; set; }

        [Required]
        public DateOnly StartDate { get; set; }
        [Required]
        public DateOnly EndDate { get; set; }
    }
} 