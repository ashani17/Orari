using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace Orari.Services
{
    public class JwtTokenGenerator : IJwtTokenGenerator
    {
        private readonly JwtSettings _jwtSettings;
        private readonly ILogger<JwtTokenGenerator> _logger;

        public JwtTokenGenerator(IOptions<JwtSettings> jwtSettings, ILogger<JwtTokenGenerator> logger)
        {
            _jwtSettings = jwtSettings?.Value ?? throw new ArgumentNullException(nameof(jwtSettings));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            // Log JWT settings (excluding the secret for security)
            _logger.LogInformation("JwtTokenGenerator initialized with settings: Issuer={Issuer}, Audience={Audience}, ExpiryMinutes={ExpiryMinutes}",
                _jwtSettings.Issuer,
                _jwtSettings.Audience,
                _jwtSettings.ExpiryMinutes);

            if (string.IsNullOrEmpty(_jwtSettings.Secret))
            {
                _logger.LogError("JWT Secret is null or empty");
                throw new ArgumentException("JWT Secret cannot be null or empty", nameof(jwtSettings));
            }

            if (string.IsNullOrEmpty(_jwtSettings.Issuer))
            {
                _logger.LogError("JWT Issuer is null or empty");
                throw new ArgumentException("JWT Issuer cannot be null or empty", nameof(jwtSettings));
            }

            if (string.IsNullOrEmpty(_jwtSettings.Audience))
            {
                _logger.LogError("JWT Audience is null or empty");
                throw new ArgumentException("JWT Audience cannot be null or empty", nameof(jwtSettings));
            }

            if (_jwtSettings.ExpiryMinutes <= 0)
            {
                _logger.LogError("JWT ExpiryMinutes must be greater than 0");
                throw new ArgumentException("JWT ExpiryMinutes must be greater than 0", nameof(jwtSettings));
            }
        }

        public string GenerateToken(string userId, string email)
        {
            try
            {
                _logger.LogInformation("Generating token for user {UserId} with email {Email}", userId, email);

                if (string.IsNullOrEmpty(userId))
                {
                    _logger.LogError("UserId is null or empty");
                    throw new ArgumentNullException(nameof(userId));
                }

                if (string.IsNullOrEmpty(email))
                {
                    _logger.LogError("Email is null or empty");
                    throw new ArgumentNullException(nameof(email));
                }

                var signingCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret)),
                    SecurityAlgorithms.HmacSha256);

                var claims = new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, userId),
                    new Claim(JwtRegisteredClaimNames.Email, email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

                var securityToken = new JwtSecurityToken(
                    issuer: _jwtSettings.Issuer,
                    audience: _jwtSettings.Audience,
                    expires: DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiryMinutes),
                    claims: claims,
                    signingCredentials: signingCredentials);

                _logger.LogInformation("Token generated successfully for user {UserId}", userId);
                return new JwtSecurityTokenHandler().WriteToken(securityToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating token for user {UserId}", userId);
                throw;
            }
        }
    }
}
