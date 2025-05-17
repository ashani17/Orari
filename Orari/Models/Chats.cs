using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Orari.Models
{
    public class Chats
    {
        [Key]
        public int CHId { get; set; }
        
        [ForeignKey("PId")]
        public required int PId { get; set; }
        public Profesors Profesor { get; set; } = null!;
        
        [ForeignKey("AId")]
        public required int AId { get; set; }
        public Profesors Admin { get; set; } = null!;
        
        public DateTime CreatedAt { get; set; }
        public ICollection<Messages> Messages { get; set; } = new List<Messages>();
    }
}
