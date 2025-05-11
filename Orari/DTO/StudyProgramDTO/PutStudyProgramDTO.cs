namespace Orari.DTO.StudyProgramDTO
{
    public class PutStudyProgramDTO
    {
        PutStudyProgramDTO() { }
        public int SPId { get; set; }
        public string SPName { get; set; } = string.Empty;
        public int DId { get; set; } // Foreign key to Departments
        public string DName { get; set; } = string.Empty; // Navigation property
        public PutStudyProgramDTO(int sPId, string sPName, int dId, string dName)
        {
            SPName = sPName;
            DName = dName;
        }
    }
}
