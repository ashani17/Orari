using System.ComponentModel.DataAnnotations;

namespace Orari.Models
{
    public class Rooms
    {
        [Key]
        public int RId { get; set; }
        public required string RName { get; set; }
        public required int RCapacity { get; set; }
        public required string RType { get; set; }
        public required string RDescription { get; set; }
    }
}
