namespace Orari.Models
{
    public class Courses
    {
        public int CId { get; set; }
        public required string Name { get; set; }
        public int Credits { get; set; }
        public required int PId { get; set; }
        public required string Professor { get; set; }
        public required IComparable<Enrollments> Enrollments { get; set; }
    }
}
