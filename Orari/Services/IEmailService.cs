namespace Orari.Services
{
    public interface IEmailService
    {
        Task SendEmailAsync(string to, string subject, string body);
        Task SendEmailConfirmationAsync(string to, string callbackUrl, string userName);
        Task SendPasswordResetAsync(string to, string callbackUrl, string userName);
    }
} 