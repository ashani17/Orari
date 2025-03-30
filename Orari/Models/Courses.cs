using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Orari.Models
{
    public class Courses
    {
        [Key]
        public int CId { get; set; }
        public required string CName { get; set; }
        public int Credits { get; set; }
        [ForeignKey("PId")]
        public required int PId { get; set; }
        public required string Professor { get; set; }
        public required ICollection<Enrollments> Enrollments { get; set; }
    }
}
