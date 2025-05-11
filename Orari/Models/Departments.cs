using System.ComponentModel.DataAnnotations;

namespace Orari.Models
{
    public class Departments
    {
        [Key]
        public int DId { get; set; }
        public string DName { get; set; }

        public ICollection<StudyPrograms> StudyPrograms { get; set; } = new List<StudyPrograms>();
    }
}
