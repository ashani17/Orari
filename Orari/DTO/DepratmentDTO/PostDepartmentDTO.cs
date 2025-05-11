using System.ComponentModel.DataAnnotations;

namespace Orari.DTO.DepratmentDTO
{
    public class PostDepartmentDTO
    {
        PostDepartmentDTO() 
        {
        }
        public PostDepartmentDTO(string dName)
        {
            DName = dName;
        }
        [Required]
        public required string DName { get; set; } = string.Empty;
    }
}
