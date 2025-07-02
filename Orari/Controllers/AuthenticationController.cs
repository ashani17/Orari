using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Orari.DTO.AuthenticationDTO;
using Orari.Models;
using Orari.Services;
using Orari.ViewModels;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;
using QuestPDF.Infrastructure;

namespace Orari.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthenticationController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly ILogger<AuthenticationController> _logger;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly IEmailService _emailService;
        private readonly IDomainValidationService _domainValidationService;

        public AuthenticationController(
            UserManager<User> userManager, 
            SignInManager<User> signInManager, 
            IJwtTokenGenerator jwtTokenGenerator,
            ILogger<AuthenticationController> logger,
            RoleManager<IdentityRole> roleManager,
            IConfiguration configuration,
            IEmailService emailService,
            IDomainValidationService domainValidationService)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
            _jwtTokenGenerator = jwtTokenGenerator ?? throw new ArgumentNullException(nameof(jwtTokenGenerator));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _roleManager = roleManager ?? throw new ArgumentNullException(nameof(roleManager));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _emailService = emailService ?? throw new ArgumentNullException(nameof(emailService));
            _domainValidationService = domainValidationService ?? throw new ArgumentNullException(nameof(domainValidationService));

            QuestPDF.Settings.License = LicenseType.Community;
        }

        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDTO model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Validate email domain
            if (!_domainValidationService.IsValidDomain(model.Email))
            {
                _logger.LogWarning("Registration attempt failed: Invalid email domain {Email}", model.Email);
                return BadRequest(_domainValidationService.GetDomainValidationMessage());
            }

            try
            {
                // Check if user already exists
                var existingUser = await _userManager.FindByEmailAsync(model.Email);
                if (existingUser != null)
                {
                    _logger.LogWarning("Registration attempt failed: User already exists with email {Email}", model.Email);
                    return BadRequest("User with this email already exists.");
                }

                // Create User with Student role (only students can register through this endpoint)
                // Professors and Admins must be created by an admin through the admin panel
                var user = new User
                {
                    UserName = model.Email,
                    Email = model.Email,
                    FirstName = model.Name,
                    LastName = model.LastName,
                    EmailConfirmed = false,
                    PhoneNumberConfirmed = true,
                    TwoFactorEnabled = false,
                    LockoutEnabled = false,
                    AccessFailedCount = 0,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };
                var result = await _userManager.CreateAsync(user, model.Password);
                
                if (!result.Succeeded)
                {
                    var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                    _logger.LogWarning("Registration failed for email {Email}: {Errors}", model.Email, errors);
                    return BadRequest(errors);
                }

                _logger.LogInformation("Student created successfully. Adding claims for student {StudentId}", user.Id);

                try
                {
                    await _userManager.AddClaimAsync(user, new Claim("FirstName", model.Name));
                    await _userManager.AddClaimAsync(user, new Claim("LastName", model.LastName));
                    _logger.LogInformation("Claims added successfully");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error adding claims for student {StudentId}", user.Id);
                    // Continue with the process even if claims fail
                }

                // Always assign Student role for public registration
                // Professors and Admins can only be created by admins through the admin panel
                await _userManager.AddToRoleAsync(user, "Student");

                await _signInManager.SignInAsync(user, isPersistent: false);
                _logger.LogInformation("Student signed in successfully");

                try
                {
                    _logger.LogInformation("Generating token for student {StudentId}", user.Id);
                    
                    // Get user roles
                    var roles = await _userManager.GetRolesAsync(user);
                    var token = _jwtTokenGenerator.GenerateToken(user.Id, user.Email!, roles);
                    _logger.LogInformation("Token generated successfully");
                    
                    var role = roles.FirstOrDefault() ?? "Student";
                    
                    var authResponse = new AuthResponse
                    {
                        Token = token,
                        User = new UserResponse
                        {
                            Id = user.Id,
                            Email = user.Email!,
                            name = user.FirstName,
                            surname = user.LastName,
                            Role = role,
                            EmailConfirmed = user.EmailConfirmed
                        }
                    };
                    
                    return Ok(authResponse);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error generating token for student {StudentId}", user.Id);
                    return StatusCode(500, "Error generating authentication token");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error during registration for email {Email}", model.Email);
                return StatusCode(500, "An unexpected error occurred during registration.");
            }
        }

        [HttpPost("login")]
        [ProducesResponseType(typeof(AuthResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO loginRequestDTO)
        {
            _logger.LogInformation("Login endpoint called with email: {Email}", loginRequestDTO?.Email);

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid login request model state: {Errors}", 
                    string.Join(", ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)));
                return BadRequest(ModelState);
            }

            var user = await _userManager.Users.FirstOrDefaultAsync(s => s.Email == loginRequestDTO.Email);
            if (user == null)
            {
                _logger.LogWarning("Login attempt failed: Student not found with SEmail {SEmail}", loginRequestDTO.Email);
                return Unauthorized("Invalid email or password.");
            }

            // Check if email is confirmed
            if (!user.EmailConfirmed)
            {
                _logger.LogWarning("Login attempt failed: Email not confirmed for student {SEmail}", loginRequestDTO.Email);
                return BadRequest("Please confirm your email address before logging in.");
            }

            var isValid = await _userManager.CheckPasswordAsync(user, loginRequestDTO.Password);
            if (!isValid)
            {
                _logger.LogWarning("Login attempt failed: Invalid password for student {SEmail}", loginRequestDTO.Email);
                return Unauthorized("Invalid email or password.");
            }

            try
            {
                _logger.LogInformation("Generating token for student {StudentId} with SEmail {SEmail}", user.Id, user.Email);
                
                // Get user roles
                var roles = await _userManager.GetRolesAsync(user);
                var token = _jwtTokenGenerator.GenerateToken(user.Id, user.Email!, roles);
                
                var role = roles.FirstOrDefault() ?? "Student";
                
                var authResponse = new AuthResponse
                {
                    Token = token,
                    User = new UserResponse
                    {
                        Id = user.Id,
                        Email = user.Email!,
                        name = user.FirstName,
                        surname = user.LastName,
                        Role = role,
                        EmailConfirmed = user.EmailConfirmed
                    }
                };
                
                return Ok(authResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating authentication token for student {StudentId}", user.Id);
                return StatusCode(500, "Error generating authentication token.");
            }
        }

        [HttpPost("logout")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Ok(new { Message = "Logged out successfully" });
        }

        [HttpPost("create-first-admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateFirstAdmin([FromBody] CreateFirstAdminDTO admin)
        {
            // Check if any admin already exists
            var existingAdmins = await _userManager.GetUsersInRoleAsync("Admin");
            if (existingAdmins.Any())
            {
                return BadRequest("Admin user already exists. This endpoint is only for creating the first admin.");
            }

            if (admin == null)
            {
                return BadRequest("Admin data is required");
            }

            // Create Admin User
            var user = new User
            {
                UserName = admin.Email,
                Email = admin.Email,
                FirstName = admin.FirstName,
                LastName = admin.LastName,
                EmailConfirmed = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            var result = await _userManager.CreateAsync(user, admin.Password);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            // Create Admin role if it doesn't exist
            if (!await _roleManager.RoleExistsAsync("Admin"))
            {
                await _roleManager.CreateAsync(new IdentityRole("Admin"));
            }

            // Add to Admin role
            await _userManager.AddToRoleAsync(user, "Admin");

            // Generate token with roles
            var roles = await _userManager.GetRolesAsync(user);
            var token = _jwtTokenGenerator.GenerateToken(user.Id, user.Email!, roles);
            
            var authResponse = new AuthResponse
            {
                Token = token,
                User = new UserResponse
                {
                    Id = user.Id,
                    Email = user.Email!,
                    name = user.FirstName,
                    surname = user.LastName,
                    Role = "Admin",
                    EmailConfirmed = user.EmailConfirmed
                }
            };

            return Ok(authResponse);
        }

        [HttpPost("confirm-email")]
        public async Task<IActionResult> ConfirmEmail([FromBody] ConfirmEmailDTO model)
        {
            if (string.IsNullOrEmpty(model.UserId) || string.IsNullOrEmpty(model.Token))
            {
                return BadRequest("Invalid confirmation link.");
            }

            var user = await _userManager.FindByIdAsync(model.UserId);
            if (user == null)
            {
                return BadRequest("Invalid confirmation link.");
            }

            var result = await _userManager.ConfirmEmailAsync(user, model.Token);
            if (result.Succeeded)
            {
                return Ok(new EmailConfirmationResponseDTO 
                { 
                    Success = true, 
                    Message = "Email confirmed successfully. You can now log in." 
                });
            }

            return BadRequest(new EmailConfirmationResponseDTO 
            { 
                Success = false, 
                Message = "Email confirmation failed. Please try again or contact support." 
            });
        }

        [HttpPost("resend-confirmation")]
        public async Task<IActionResult> ResendConfirmationEmail([FromBody] EmailConfirmationRequestDTO model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return BadRequest("User not found.");
            }

            if (user.EmailConfirmed)
            {
                return BadRequest("Email is already confirmed.");
            }

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var confirmationLink = Url.Action("ConfirmEmail", "Authentication", 
                new { userId = user.Id, token = token }, Request.Scheme);

            try
            {
                await _emailService.SendEmailConfirmationAsync(user.Email, confirmationLink, user.FirstName);
                return Ok(new EmailConfirmationResponseDTO 
                { 
                    Success = true, 
                    Message = "Confirmation email sent successfully." 
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new EmailConfirmationResponseDTO 
                { 
                    Success = false, 
                    Message = "Failed to send confirmation email. Please try again later." 
                });
            }
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] EmailConfirmationRequestDTO model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                // Don't reveal that the user doesn't exist
                return Ok(new EmailConfirmationResponseDTO 
                { 
                    Success = true, 
                    Message = "If your email is registered, you will receive a password reset link." 
                });
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var resetLink = Url.Action("ResetPassword", "Authentication", 
                new { userId = user.Id, token = token }, Request.Scheme);

            try
            {
                await _emailService.SendPasswordResetAsync(user.Email, resetLink, user.FirstName);
                return Ok(new EmailConfirmationResponseDTO 
                { 
                    Success = true, 
                    Message = "Password reset email sent successfully." 
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new EmailConfirmationResponseDTO 
                { 
                    Success = false, 
                    Message = "Failed to send password reset email. Please try again later." 
                });
            }
        }

        [HttpPost("reset-password")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDTO model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _userManager.FindByIdAsync(model.UserId);
            if (user == null)
            {
                return BadRequest("Invalid user.");
            }

            var result = await _userManager.ResetPasswordAsync(user, model.Token, model.NewPassword);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return BadRequest(errors);
            }

            return Ok(new { message = "Password reset successfully." });
        }

        [HttpPut("update-profile")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileDTO model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User not authenticated.");
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            // Update basic fields
            if (!string.IsNullOrEmpty(model.FirstName))
                user.FirstName = model.FirstName;
            if (!string.IsNullOrEmpty(model.LastName))
                user.LastName = model.LastName;
            if (!string.IsNullOrEmpty(model.Phone))
                user.Phone = model.Phone;
            
            user.UpdatedAt = DateTime.UtcNow;

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return BadRequest(errors);
            }

            // Return updated user information
            var roles = await _userManager.GetRolesAsync(user);
            var role = roles.FirstOrDefault() ?? "Student";
            
            var userResponse = new UserResponse
            {
                Id = user.Id,
                Email = user.Email!,
                name = user.FirstName ?? "",
                surname = user.LastName ?? "",
                Role = role,
                EmailConfirmed = user.EmailConfirmed,
                Phone = user.Phone
            };

            return Ok(userResponse);
        }
    }

    public class TokenResponse
    {
        public string Token { get; set; } = string.Empty;
    }

    public class AuthResponse
    {
        public string Token { get; set; } = string.Empty;
        public UserResponse User { get; set; } = new();
    }

    public class UserResponse
    {
        public string Id { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string name { get; set; } = string.Empty;
        public string surname { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public bool EmailConfirmed { get; set; } = false;
        public string? Phone { get; set; }
    }

    public class CreateFirstAdminDTO
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
    }

    public class ResetPasswordDTO
    {
        public string UserId { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
        public string NewPassword { get; set; } = string.Empty;
    }

    public class UpdateProfileDTO
    {
        [StringLength(50)]
        public string? FirstName { get; set; }

        [StringLength(50)]
        public string? LastName { get; set; }

        [StringLength(20)]
        public string? Phone { get; set; }
    }
}
