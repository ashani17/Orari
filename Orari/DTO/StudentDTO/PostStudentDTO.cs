using Orari.Models;
using System.ComponentModel.DataAnnotations;
using System.Numerics;

namespace Orari.DTO.StudentDTO
{
    public class PostStudentDTO
    {
        public PostStudentDTO()
        {
        }

        public PostStudentDTO(string name, string surname, string email, string password)
        {
            SName = name;
            SSurname = surname;
            SEmail = email;
            SPassword = password;
        }
        [Required]
        [StringLength(50, ErrorMessage = "Name cannot be longer than 50 characters.")]
        public string SName { get; set; } = string.Empty;
        [Required]
        [StringLength(50, ErrorMessage = "Surname cannot be longer than 50 characters.")]
        public string SSurname { get; set; } = string.Empty;
        [Required]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string SEmail { get; set; } = string.Empty;
        [Required]
        [StringLength(20, ErrorMessage = "Password cannot be longer than 20 characters.")]
        public string SPassword { get; set; } = string.Empty;
        public ICollection<Enrollments> Enrollments { get; set; } = new List<Enrollments>();
        public DateTime SCreatedAt { get; set; } = DateTime.Now;


    }
}
