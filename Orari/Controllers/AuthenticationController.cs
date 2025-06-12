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
        private readonly UserManager<Students> _userManager;
        private readonly SignInManager<Students> _signInManager;
        private readonly JwtTokenGenerator _jwtTokenGenerator;
        private readonly ILogger<AuthenticationController> _logger;

        public AuthenticationController(
            UserManager<Students> userManager, 
            SignInManager<Students> signInManager, 
            JwtTokenGenerator jwtTokenGenerator,
            ILogger<AuthenticationController> logger)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
            _jwtTokenGenerator = jwtTokenGenerator ?? throw new ArgumentNullException(nameof(jwtTokenGenerator));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Register([FromBody] RegisterViewModel model)
        {
            _logger.LogInformation("Register endpoint called with email: {Email}", model?.Email);

            try
            {
                if (model == null)
                {
                    _logger.LogError("Register model is null");
                    return BadRequest("Invalid request data");
                }

                _logger.LogInformation("Starting registration process for email: {Email}", model.Email);

                if (!ModelState.IsValid)
                {
                    var errors = string.Join(", ", ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage));
                    _logger.LogWarning("Invalid registration model state: {Errors}", errors);
                    return BadRequest(ModelState);
                }

                if (string.IsNullOrEmpty(model.Email))
                {
                    _logger.LogError("Email is null or empty");
                    return BadRequest("Email is required");
                }

                if (string.IsNullOrEmpty(model.Password))
                {
                    _logger.LogError("Password is null or empty");
                    return BadRequest("Password is required");
                }

                if (string.IsNullOrEmpty(model.Name))
                {
                    _logger.LogError("Name is null or empty");
                    return BadRequest("Name is required");
                }

                if (string.IsNullOrEmpty(model.LastName))
                {
                    _logger.LogError("LastName is null or empty");
                    return BadRequest("LastName is required");
                }

                var student = new Students 
                { 
                    UserName = model.Email,
                    Email = model.Email,
                    SName = model.Name,
                    SSurname = model.LastName,
                    SCreatedAt = DateTime.UtcNow,
                    SUpdatedAt = DateTime.UtcNow,
                    SPassword = model.Password,
                    SEmail = model.Email
                };

                _logger.LogInformation("Creating new student with SEmail: {SEmail}", model.Email);
                var result = await _userManager.CreateAsync(student, model.Password);
                
                if (!result.Succeeded)
                {
                    var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                    _logger.LogWarning("Student creation failed: {Errors}", errors);
                    return BadRequest(result.Errors);
                }

                _logger.LogInformation("Student created successfully. Adding claims for student {StudentId}", student.Id);

                try
                {
                    await _userManager.AddClaimAsync(student, new Claim("FirstName", model.Name));
                    await _userManager.AddClaimAsync(student, new Claim("LastName", model.LastName));
                    _logger.LogInformation("Claims added successfully");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error adding claims for student {StudentId}", student.Id);
                    // Continue with the process even if claims fail
                }

                await _signInManager.SignInAsync(student, isPersistent: false);
                _logger.LogInformation("Student signed in successfully");

                try
                {
                    _logger.LogInformation("Generating token for student {StudentId}", student.Id);
                    var token = _jwtTokenGenerator.GenerateToken(student.Id, student.SEmail!);
                    _logger.LogInformation("Token generated successfully");
                    return Ok(new { Message = "Registration successful", Token = token });
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error generating token for student {StudentId}", student.Id);
                    return StatusCode(500, "Error generating authentication token");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error during registration for email {Email}", model?.Email);
                return StatusCode(500, "An unexpected error occurred during registration");
            }
        }

        [HttpPost("login")]
        [ProducesResponseType(typeof(TokenResponse), StatusCodes.Status200OK)]
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

            var student = await _userManager.Users.FirstOrDefaultAsync(s => s.SEmail == loginRequestDTO.Email);
            if (student == null)
            {
                _logger.LogWarning("Login attempt failed: Student not found with SEmail {SEmail}", loginRequestDTO.Email);
                return Unauthorized("Invalid email or password.");
            }

            var isValid = await _userManager.CheckPasswordAsync(student, loginRequestDTO.Password);
            if (!isValid)
            {
                _logger.LogWarning("Login attempt failed: Invalid password for student {SEmail}", loginRequestDTO.Email);
                return Unauthorized("Invalid email or password.");
            }

            try
            {
                _logger.LogInformation("Generating token for student {StudentId} with SEmail {SEmail}", student.Id, student.SEmail);
                var token = _jwtTokenGenerator.GenerateToken(student.Id, student.SEmail!);
                return Ok(new TokenResponse { Token = token });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating authentication token for student {StudentId}", student.Id);
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
    }

    public class TokenResponse
    {
        public string Token { get; set; } = string.Empty;
    }
}
