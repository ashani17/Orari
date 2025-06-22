using System.ComponentModel.DataAnnotations;

namespace Orari.DTO.AdminDTO
{
    public class UpdateUserDTO
    {
        [EmailAddress]
        public string? Email { get; set; }

        [StringLength(100, MinimumLength = 6)]
        public string? Password { get; set; }

        [StringLength(50)]
        public string? FirstName { get; set; }

        [StringLength(50)]
        public string? LastName { get; set; }

        [StringLength(20)]
        public string? Phone { get; set; }
    }
} 