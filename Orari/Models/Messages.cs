using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Orari.Models
{
    public class Messages
    {
        [Key]
        public int MId { get; set; }
        
        [ForeignKey("CHId")]
        public required int CHId { get; set; }
        public Chats Chat { get; set; } = null!;
        
        public required string Content { get; set; }
        public required int SenderId { get; set; }
        public required string SenderRole { get; set; }
        public DateTime SentAt { get; set; }
        public bool IsRead { get; set; }
    }
} 