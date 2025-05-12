using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Orari.DTO.AuthenticationDTO;
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

        public AuthenticationController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager,JwtTokenGenerator jwtTokenGenerator)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtTokenGenerator = jwtTokenGenerator;
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

                var token = _jwtTokenGenerator.GenerateToken(user.Id, user.Email!);
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

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new IdentityUser { UserName = model.Email, Email = model.Email };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Index", "Home");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return Ok(model);
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
