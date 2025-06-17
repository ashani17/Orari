using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Orari.DTO.AuthenticationDTO;
using Orari.Models;
using Orari.Services;
using Orari.ViewModels;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;

namespace Orari.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthenticationController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly JwtTokenGenerator _jwtTokenGenerator;
        private readonly ILogger<AuthenticationController> _logger;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AuthenticationController(
            UserManager<User> userManager, 
            SignInManager<User> signInManager, 
            JwtTokenGenerator jwtTokenGenerator,
            ILogger<AuthenticationController> logger,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
            _jwtTokenGenerator = jwtTokenGenerator ?? throw new ArgumentNullException(nameof(jwtTokenGenerator));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _roleManager = roleManager ?? throw new ArgumentNullException(nameof(roleManager));
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
                    EmailConfirmed = true,
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
                            Name = user.FirstName,
                            Surname = user.LastName,
                            Role = role
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
                        Name = user.FirstName,
                        Surname = user.LastName,
                        Role = role
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
                    Name = user.FirstName,
                    Surname = user.LastName,
                    Role = "Admin"
                }
            };

            return Ok(authResponse);
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
        public string Name { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
    }

    public class CreateFirstAdminDTO
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
    }
}
