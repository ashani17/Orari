namespace Orari.DTO.StudyProgramDTO
{
    public class PostStudyProgramDTO
    {
        PostStudyProgramDTO() { }
        public int SPId { get; set; }
        public string SPName { get; set; } = string.Empty;
        public int DId { get; set; } // Foreign key to Departments
        public string DName { get; set; } = string.Empty; // Navigation property
        public PostStudyProgramDTO(int sPId, string sPName, int dId, string dName)
        {
            SPName = sPName;
            DName = dName;
        }
    }
}
