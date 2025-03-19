namespace Orari.Models
{
    public class Profesors
    {
        public int PId { get; set; }
        public required string PName { get; set; }
        public bool Availability { get; set; }
        public string? SpecialRequirements { get; set; }

    }
}
