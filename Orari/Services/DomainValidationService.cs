using System.Text.RegularExpressions;

namespace Orari.Services
{
    public interface IDomainValidationService
    {
        bool IsValidDomain(string email);
        string GetRoleFromEmail(string email);
        string GetDomainValidationMessage();
    }

    public class DomainValidationService : IDomainValidationService
    {
        private readonly string[] _allowedDomains = { "fshn.edu.al", "fshnstudent.info" };
        private readonly string[] _adminDomains = { "admin.com", "fshn.edu.al" }; // admin.com for testing, fshn.edu.al for production
        private readonly string[] _professorDomains = { "fshn.edu.al" };

        public bool IsValidDomain(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                var domain = email.Split('@').LastOrDefault()?.ToLower();
                return !string.IsNullOrEmpty(domain) && _allowedDomains.Contains(domain);
            }
            catch
            {
                return false;
            }
        }

        public string GetRoleFromEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return "Student";

            try
            {
                var domain = email.Split('@').LastOrDefault()?.ToLower();
                
                if (string.IsNullOrEmpty(domain))
                    return "Student";

                // Check for admin domains
                if (_adminDomains.Contains(domain))
                {
                    // Additional check: if it contains "admin" in the email, it's an admin
                    if (email.ToLower().Contains("admin"))
                        return "Admin";
                }

                // Check for professor domains
                if (_professorDomains.Contains(domain))
                {
                    // Additional check: if it contains "professor" or "prof" in the email, it's a professor
                    if (email.ToLower().Contains("professor") || email.ToLower().Contains("prof"))
                        return "Professor";
                }

                // Default to Student for all other valid domains
                return "Student";
            }
            catch
            {
                return "Student";
            }
        }

        public string GetDomainValidationMessage()
        {
            return $"Only emails from the following domains are allowed: {string.Join(", ", _allowedDomains)}";
        }
    }
} 