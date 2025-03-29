using System.ComponentModel.DataAnnotations;

namespace Orari.Models
{
    public class Rooms
    {
        [Key]
        public int RId { get; set; }
        public required string RName { get; set; }
        public required int Capacity { get; set; }
        public required string RoomDescription { get; set; }
    }
}
