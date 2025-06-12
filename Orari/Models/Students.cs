using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Orari.Models
{
    public class Students : IdentityUser
    {
        public required string SName { get; set; }
        public required string SSurname { get; set; }
        public required string SPassword { get; set; }
        public required string SEmail { get; set; }
        public required DateTime SCreatedAt { get; set; }
        public DateTime SUpdatedAt { get; internal set; }
    }
}
