namespace Orari.DTO.AuthenticationDTO
{
    public class RegisterResponseDTO
    {
        public bool Succeeded { get; set; }
        public string Message { get; set; } = string.Empty;
        public string? Token { get; set; }
        public string? UserType { get; set; }
    }
} 