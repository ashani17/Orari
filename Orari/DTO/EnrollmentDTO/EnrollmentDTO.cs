using System.ComponentModel.DataAnnotations;

namespace Orari.DTO.EnrollmentDTO
{
    public class EnrollmentDTO
    {
        public EnrollmentDTO()
        {


        }
        public EnrollmentDTO(int sId, int cId)
        {
            SId = sId;
            CId = cId;
        }
        [Required]
        public int SId { get; set; }
        [Required]
        public int CId { get; set; }


    }
}
