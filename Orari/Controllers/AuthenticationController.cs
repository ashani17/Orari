using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Orari.Constants;
using Orari.DTO.AuthenticationDTO;
using Orari.Interfaces;
using Orari.Models;
using Orari.Services;
using Orari.ViewModels;

namespace Orari.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthenticationController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly JwtTokenGenerator _jwtTokenGenerator;
        private readonly IProfesorService _profesorService;
        private readonly IStudentService _studentService;

        public AuthenticationController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, JwtTokenGenerator jwtTokenGenerator, IProfesorService profesorService, IStudentService studentService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtTokenGenerator = jwtTokenGenerator;
            _profesorService = profesorService;
            _studentService = studentService;
        }

        [HttpGet("login")]
        public IActionResult LoginPage()
        {
            return Ok();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO loginRequestDTO)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(loginRequestDTO.Email);
                if (user == null)
                    return Unauthorized("Invalid email or password.");

                var isValid = await _userManager.CheckPasswordAsync(user, loginRequestDTO.Password);
                if (!isValid)
                    return Unauthorized("Invalid email or password.");

                var token = await _jwtTokenGenerator.GenerateToken(user.Id, user.Email!);
                return Ok(new { Token = token });
            }

            // Return BadRequest if ModelState is invalid
            return BadRequest("Invalid login request.");
        }

        [HttpGet("register")]
        public IActionResult RegisterPage()
        {
            return Ok();
        }

        [HttpPost("register/student")]
        public async Task<IActionResult> RegisterStudent([FromBody] StudentRegisterDTO model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!model.Email.EndsWith("@fshn.edu.al"))
                return BadRequest("Only @fshn.edu.al email addresses are allowed.");

            var user = new IdentityUser { UserName = model.Email, Email = model.Email };
            var result = await _userManager.CreateAsync(user, model.Password);
            
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, UserRoles.Student);

                // Create student record in Students table
                var student = new Students
                {
                    SName = model.Name,
                    SSurname = model.Surname,
                    SEmail = model.Email,
                    SPassword = model.Password, // Consider if you really need to store this
                    SCreatedAt = DateTime.UtcNow,
                    SUpdatedAt = DateTime.UtcNow
                };

                try 
                {
                    await _studentService.CreateStudentAsync(student);
                }
                catch (Exception ex)
                {
                    // If student creation fails, remove the Identity user
                    await _userManager.DeleteAsync(user);
                    return BadRequest($"Failed to create student record: {ex.Message}");
                }

                return Ok(new RegisterResponseDTO
                {
                    Message = "Registration successful. Please login to continue.",
                    UserType = UserRoles.Student
                });
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            return BadRequest(ModelState);
        }

        [HttpPost("register/staff")]
        [Authorize(Roles = $"{UserRoles.Admin},{UserRoles.SuperAdmin}")]
        public async Task<IActionResult> RegisterStaff([FromBody] AdminProfesorRegisterDTO model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!model.Email.EndsWith("@fshn.edu.al"))
                return BadRequest("Only @fshn.edu.al email addresses are allowed.");

            // Allow SuperAdmin to create any type of user
            if (User.IsInRole(UserRoles.SuperAdmin))
            {
                // SuperAdmin can create any type of user
            }
            else if (User.IsInRole(UserRoles.Admin))
            {
                // Regular admin restrictions
                if (model.UserType == UserRoles.SuperAdmin)
                    return BadRequest("Regular admins cannot create SuperAdmin accounts");
            }

            var user = new IdentityUser { UserName = model.Email, Email = model.Email };
            var result = await _userManager.CreateAsync(user, model.Password);
            
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, model.UserType);

                if (model.UserType == UserRoles.Professor)
                {
                    // Create professor profile with all required fields
                    var professor = new Profesors
                    {
                        PName = model.Name,
                        PSurname = model.Surname,
                        PEmail = model.Email,
                        PPassword = model.Password, // Set the password
                        PSubject = model.Subject ?? "Not Set", // Provide default if not set
                        PCreatedAt = DateTime.UtcNow,
                        PUpdatedAt = DateTime.UtcNow,
                        Availability = model.Availability ?? true, // Default to true if not set
                    };
                    await _profesorService.CreateProfesorAsync(professor);
                }

                return Ok(new { Message = $"{model.UserType} account created successfully" });
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            return BadRequest(ModelState);
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Ok(new { Message = "Logged out successfully" });
        }
    }
}
