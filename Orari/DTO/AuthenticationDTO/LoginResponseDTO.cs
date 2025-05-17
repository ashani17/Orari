namespace Orari.DTO.AuthenticationDTO
{
    public class LoginResponseDTO
    {
        public string Token { get; set; } = string.Empty;
        public string UserType { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
    }
} 