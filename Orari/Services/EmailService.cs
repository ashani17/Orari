using System.Net.Mail;
using System.Net;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;

namespace Orari.Services
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _emailSettings;
        private readonly ILogger<EmailService> _logger;

        public EmailService(IOptions<EmailSettings> emailSettings, ILogger<EmailService> logger)
        {
            _emailSettings = emailSettings.Value;
            _logger = logger;
        }

        public async Task SendEmailAsync(string to, string subject, string body)
        {
            try
            {
                using var client = new SmtpClient(_emailSettings.SmtpServer, _emailSettings.SmtpPort)
                {
                    EnableSsl = true,
                    Credentials = new NetworkCredential(_emailSettings.SmtpUsername, _emailSettings.SmtpPassword)
                };

                var message = new MailMessage
                {
                    From = new MailAddress(_emailSettings.SenderEmail, _emailSettings.SenderName),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true
                };
                message.To.Add(to);

                await client.SendMailAsync(message);
                _logger.LogInformation("Email sent successfully to {Email}", to);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send email to {Email}", to);
                throw new Exception($"Failed to send email: {ex.Message}");
            }
        }

        public async Task SendEmailConfirmationAsync(string to, string callbackUrl, string userName)
        {
            var subject = "Confirm your email address - Orari University";
            var body = $@"
                <html>
                <body>
                    <h2>Welcome to Orari University!</h2>
                    <p>Hello {userName},</p>
                    <p>Thank you for registering with Orari University. Please confirm your email address by clicking the link below:</p>
                    <p><a href='{callbackUrl}' style='background-color: #007bff; color: white; padding: 10px 20px; text-decoration: none; border-radius: 5px;'>Confirm Email Address</a></p>
                    <p>If the button doesn't work, you can copy and paste this link into your browser:</p>
                    <p>{callbackUrl}</p>
                    <p>This link will expire in 24 hours.</p>
                    <p>If you didn't create an account with us, please ignore this email.</p>
                    <br>
                    <p>Best regards,<br>Orari University Team</p>
                </body>
                </html>";

            await SendEmailAsync(to, subject, body);
        }

        public async Task SendPasswordResetAsync(string to, string callbackUrl, string userName)
        {
            var subject = "Reset your password - Orari University";
            var body = $@"
                <html>
                <body>
                    <h2>Password Reset Request</h2>
                    <p>Hello {userName},</p>
                    <p>We received a request to reset your password. Click the link below to create a new password:</p>
                    <p><a href='{callbackUrl}' style='background-color: #dc3545; color: white; padding: 10px 20px; text-decoration: none; border-radius: 5px;'>Reset Password</a></p>
                    <p>If the button doesn't work, you can copy and paste this link into your browser:</p>
                    <p>{callbackUrl}</p>
                    <p>This link will expire in 1 hour.</p>
                    <p>If you didn't request a password reset, please ignore this email.</p>
                    <br>
                    <p>Best regards,<br>Orari University Team</p>
                </body>
                </html>";

            await SendEmailAsync(to, subject, body);
        }
    }
} 