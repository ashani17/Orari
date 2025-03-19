namespace Orari.Models
{
    public class Students
    {
        public int SId { get; set; }
        public required string Name { get; set; }
        public required string Email { get; set; }
        public required ICollection<Enrollments> Enrollments { get; set; }
    }
}
