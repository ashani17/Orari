using Orari.DTO.ProfesorDTO;

namespace Orari.DTO.CoursesDTO
{
    public class PostCourseDTO
    {
        public required string CName { get; set; }
        public int Credits { get; set; }
        public required ProfesorForCourseDTO Profesor { get; set; }
        public int StudyProgramId { get; set; }
    }

    public class ProfesorForCourseDTO
    {
        public int PId { get; set; }
        public string PEmail { get; set; }
    }
}
