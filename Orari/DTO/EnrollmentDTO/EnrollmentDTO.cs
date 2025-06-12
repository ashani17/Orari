using System.ComponentModel.DataAnnotations;

namespace Orari.DTO.EnrollmentDTO
{
    public class EnrollmentDTO
    {
        public EnrollmentDTO()
        {


        }
        public EnrollmentDTO(string studentId, int cId)
        {
            StudentId = studentId;
            CId = cId;
        }
        [Required]
        public string StudentId { get; set; }
        [Required]
        public int CId { get; set; }


    }
}
