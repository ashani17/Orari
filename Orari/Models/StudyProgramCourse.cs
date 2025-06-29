using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Orari.Models
{
    public class StudyProgramCourse
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey ("SPId")]
        public int SPId { get; set; }
        public StudyPrograms StudyProgram { get; set; }
        [ForeignKey("CId")]
        public int CId { get; set; }
        public Courses Course { get; set; } 
        
        // Year field (1, 2, or 3)
        public int Year { get; set; }
        
        // Academic year field (e.g., "2023-2026")
        [StringLength(20)]
        public string AcademicYear { get; set; } = string.Empty;
    }
}
