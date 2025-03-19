namespace Orari.Models
{
    public class Enrollments
    {
        public int EId { get; set; }

        public int SId { get; set; }
        public required Students Students { get; set; }

        public int CId { get; set; }
        public required Courses Courses { get; set; }
    }
}
