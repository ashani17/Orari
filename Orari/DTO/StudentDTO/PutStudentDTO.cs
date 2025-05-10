using Orari.Models;
using System.ComponentModel.DataAnnotations;

namespace Orari.DTO.StudentDTO
{
    public class PutStudentDTO
    {
        public PutStudentDTO()
        {
        }
        public PutStudentDTO(string name, string surname, string email, string password)
        {
            SName = name;
            SSurname = surname;
            SEmail = email;
            SPassword = password;
        }

        [StringLength(50, ErrorMessage = "Name cannot be longer than 50 characters.")]
        public string SName { get; set; } = string.Empty;

        [StringLength(50, ErrorMessage = "Surname cannot be longer than 50 characters.")]
        public string SSurname { get; set; } = string.Empty;

        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string SEmail { get; set; } = string.Empty;

        [StringLength(20, ErrorMessage = "Password cannot be longer than 20 characters.")]
        public string SPassword { get; set; } = string.Empty;
        public DateTime SUpdatedAt { get; set; } = DateTime.Now;
    }
}
