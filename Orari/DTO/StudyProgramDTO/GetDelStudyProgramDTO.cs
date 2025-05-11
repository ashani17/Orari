using System.ComponentModel.DataAnnotations;

namespace Orari.DTO.StudyProgramDTO
{
    public class GetDelStudyProgramDTO
    {
        public int SPId { get; set; }

        public string SPName { get; set; } = string.Empty;

        public int DId { get; set; } // Foreign key to Departments
        public string DName { get; set; } = string.Empty; // Navigation property
    }
}
