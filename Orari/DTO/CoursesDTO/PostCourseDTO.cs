using System.Text.Json.Serialization;
using Orari.DTO.ProfesorDTO;

namespace Orari.DTO.CoursesDTO
{
    public class PostCourseDTO
    {
        [JsonPropertyName("cName")]
        public string CName { get; set; } = string.Empty;

        [JsonPropertyName("credits")]
        public int Credits { get; set; }

        [JsonPropertyName("pId")]
        public string PId { get; set; } = string.Empty;

        [JsonPropertyName("profesor")]
        public string Profesor { get; set; } = string.Empty;

        public int StudyProgramId { get; set; }
    }

    public class ProfesorForCourseDTO
    {
        public int PId { get; set; }
        public string PEmail { get; set; }
    }
}
