using Orari.DTO.ProfesorDTO;
using Orari.Models;

namespace Orari.DTO.CoursesDTO
{
    public class PostCourseDTO
    {
        public PostCourseDTO()
        {
        }
        public PostCourseDTO(string cName, int credits, PostProfesorDTO postProfesorDTO)
        {
            CName = cName;
            Credits = credits;
            Profesor = new Profesors()
            {
                PName = postProfesorDTO.PName,
                PSurname = postProfesorDTO.PSurname,
                PEmail = postProfesorDTO.PEmail,
                PPhone = postProfesorDTO.PPhone,
                PSubject = postProfesorDTO.PSubject,
                PPassword = postProfesorDTO.PPassword,
                Availability = postProfesorDTO.Availability,
                SpecialRequirements = postProfesorDTO.SpecialRequirements,
                PCreatedAt = postProfesorDTO.PCreatedAt,

            };
        }
        public required string CName { get; set; }
        public int Credits { get; set; }
        public Profesors Profesor { get; set; }
    }
}
