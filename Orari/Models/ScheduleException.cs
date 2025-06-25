using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Orari.Models
{
    public class ScheduleException
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int RecurringScheduleId { get; set; }
        [ForeignKey("RecurringScheduleId")]
        public RecurringSchedule RecurringSchedule { get; set; } = null!;

        [Required]
        public DateOnly Date { get; set; }

        public bool IsCancelled { get; set; } = false;

        // Optional overrides
        public int? NewRoomId { get; set; }
        public TimeOnly? NewStartTime { get; set; }
        public TimeOnly? NewEndTime { get; set; }
    }
} 