using System.Text.Json.Serialization;

namespace Orari.DTO.StudyProgramDTO
{
    public class PostStudyProgramDTO
    {
        [JsonPropertyName("spName")]
        public string SPName { get; set; } = string.Empty;
        
        [JsonPropertyName("dId")]
        public int DId { get; set; } // Foreign key to Departments
    }
}
