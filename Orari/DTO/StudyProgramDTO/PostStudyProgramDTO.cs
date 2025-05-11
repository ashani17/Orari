namespace Orari.DTO.StudyProgramDTO
{
    public class PostStudyProgramDTO
    {
        public string SPName { get; set; } = string.Empty;
        public int DId { get; set; } // Foreign key to Departments

    }
}
