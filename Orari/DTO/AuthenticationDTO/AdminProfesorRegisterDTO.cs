using System.ComponentModel.DataAnnotations;

namespace Orari.DTO.AuthenticationDTO
{
    public class AdminProfesorRegisterDTO
    {
        [Required]
        [EmailAddress]
        public required string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [StringLength(100, MinimumLength = 6)]
        public required string Password { get; set; }

        [Required]
        public required string UserType { get; set; } // "Admin" or "Professor"

        [Required]
        public required string Name { get; set; }

        [Required]
        public required string Surname { get; set; }

        public string? Phone { get; set; }

        // Additional fields for professors
        public string? Subject { get; set; }
        public bool? Availability { get; set; }
        public string? SpecialRequirements { get; set; }
    }
} 