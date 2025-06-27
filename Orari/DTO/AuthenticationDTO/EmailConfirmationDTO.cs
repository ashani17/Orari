namespace Orari.DTO.AuthenticationDTO
{
    public class EmailConfirmationRequestDTO
    {
        public string Email { get; set; } = string.Empty;
    }

    public class EmailConfirmationResponseDTO
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
    }

    public class ConfirmEmailDTO
    {
        public string UserId { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
    }
} 