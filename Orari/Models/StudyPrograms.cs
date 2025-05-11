using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Orari.Models
{
    public class StudyPrograms
    {
        [Key]
        public int SPId { get; set; }
        public required string SPName { get; set; }
        public int DId { get; set; } // Foreign key to Departments
        public Departments Departments { get; set; } // Navigation property
        public ICollection<StudyProgramCourse> StudyProgramCourse { get; set; } = new List<StudyProgramCourse>();
        public ICollection<Students> Students { get; set; } = new List<Students>();
    }
}
