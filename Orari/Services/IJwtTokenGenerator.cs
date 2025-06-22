namespace Orari.Services
{
    public interface IJwtTokenGenerator
    {
        string GenerateToken(string userId, string email);
        string GenerateToken(string userId, string email, IEnumerable<string> roles);
    }
} 