using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Orari.Models
{
    public class Enrollments
    {
        [Key]
        public int EId { get; set; }

        public string StudentId { get; set; } = string.Empty;
        [ForeignKey("StudentId")]
        public required Students Student { get; set; }

        public int CId { get; set; }
        [ForeignKey("CId")]
        public required Courses Courses { get; set; }
    }
}
