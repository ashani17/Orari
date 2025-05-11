using System.ComponentModel.DataAnnotations;

namespace Orari.DTO.DepratmentDTO
{
    public class PutDepartmentDTO
    {
        public PutDepartmentDTO()
        {
        }
        public PutDepartmentDTO(string dName)
        {
            DName = dName;
        }
        [Required]
        public string DName { get; set; } = string.Empty;
    }
}
