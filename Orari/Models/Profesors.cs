using System.ComponentModel.DataAnnotations;

namespace Orari.Models
{
    public class Profesors
    {
        [Key]
        public int PId { get; set; }
        public required string PName { get; set; }
        public string? PEmail { get; set; }
        public string? PPhone { get; set; }
        public required string PPassword { get; set; }
        public required string PSurname { get; set; }
        public required string PSubject { get; set; }

        public bool Availability { get; set; }
        public string? SpecialRequirements { get; set; }
        public required DateTime PCreatedAt { get; set; }
        public DateTime PUpdatedAt { get; internal set; }
    }
}
