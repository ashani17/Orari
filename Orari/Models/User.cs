using Microsoft.AspNetCore.Identity;

namespace Orari.Models
{
    public class User : IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        
        // Optional fields for professors
        public string? Subject { get; set; }
        public string? Phone { get; set; }
        public bool Availability { get; set; } = true;
        public string? SpecialRequirements { get; set; }
    }
} 