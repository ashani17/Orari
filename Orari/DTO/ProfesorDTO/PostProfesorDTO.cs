namespace Orari.DTO.ProfesorDTO
{
    public class PostProfesorDTO
    {
        public PostProfesorDTO()
        {

        }

        public PostProfesorDTO(string pName, string pSurname, string pEmail, string pPhone, string pSubject,string pPassword,bool availability, string specialrequirements)
        {
            PName = pName;
            PSurname = pSurname;
            PEmail = pEmail;
            PPhone = pPhone;
            PSubject = pSubject;
            PPassword = pPassword;
            Availability = availability;
            SpecialRequirements = specialrequirements;
        }
        public required string PName { get; set; }
        public required string PSurname { get; set; }
        public required string PEmail { get; set; }
        public string? PPhone { get; set; }
        public required string PPassword { get; set; }
        public required string PSubject { get; set; }
        public bool Availability { get; set; }
        public string? SpecialRequirements { get; set; }
        public required DateTime PCreatedAt { get; set; } = DateTime.Now;

    }
}
