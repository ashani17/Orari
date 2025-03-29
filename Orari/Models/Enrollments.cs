using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Orari.Models
{
    public class Enrollments
    {
        [Key]
        public int EId { get; set; }

        public int SId { get; set; }
        [ForeignKey("SId")]
        public required Students Students { get; set; }

        public int CId { get; set; }
        [ForeignKey("CId")]
        public required Courses Courses { get; set; }
    }
}
