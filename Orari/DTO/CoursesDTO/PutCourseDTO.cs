using Orari.DTO.ProfesorDTO;
using Orari.Models;

namespace Orari.DTO.CoursesDTO
{
    public class PutCourseDTO
    {
        public PutCourseDTO()
        {
        }
        public PutCourseDTO(string cName, int credits, int profesorId,PutProfesorDTO putProfesorDTO)
        {
            CName = cName;
            Credits = credits;
            Profesor = new Profesors()
            {
                PId = profesorId,
                PName = putProfesorDTO.PName,
                PSurname = putProfesorDTO.PSurname,
                PEmail = putProfesorDTO.PEmail,
                PPhone = putProfesorDTO.PPhone,
                PSubject = putProfesorDTO.PSubject,
                PPassword = putProfesorDTO.PPassword,
                Availability = putProfesorDTO.Availability,
                SpecialRequirements = putProfesorDTO.SpecialRequirements,
                PCreatedAt = putProfesorDTO.PUpdatedAt,
            };
        }
        public required string CName { get; set; }
        public int Credits { get; set; }
        public required Profesors Profesor { get; set; }

    }
}
