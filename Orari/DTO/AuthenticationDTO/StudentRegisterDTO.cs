using System.ComponentModel.DataAnnotations;

namespace Orari.DTO.AuthenticationDTO
{
    public class StudentRegisterDTO
    {
        [Required]
        [EmailAddress]
        public required string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [StringLength(100, MinimumLength = 6)]
        public required string Password { get; set; }

        [Required]
        [Compare("Password", ErrorMessage = "Passwords do not match.")]
        public required string ConfirmPassword { get; set; }

        [Required]
        public required string Name { get; set; }

        [Required]
        public required string Surname { get; set; }
    }
} 