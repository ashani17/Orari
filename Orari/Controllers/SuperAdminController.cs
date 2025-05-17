using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Orari.Constants;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Orari.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = UserRoles.SuperAdmin)]
    public class SuperAdminController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public SuperAdminController(
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [HttpGet("users")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userManager.Users.ToListAsync();
            var userList = new List<object>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                userList.Add(new
                {
                    Email = user.Email,
                    Roles = roles
                });
            }

            return Ok(userList);
        }

        [HttpPost("change-user-role")]
        public async Task<IActionResult> ChangeUserRole(string userEmail, string newRole)
        {
            var user = await _userManager.FindByEmailAsync(userEmail);
            if (user == null)
                return NotFound("User not found");

            var currentRoles = await _userManager.GetRolesAsync(user);
            await _userManager.RemoveFromRolesAsync(user, currentRoles);
            await _userManager.AddToRoleAsync(user, newRole);

            return Ok($"User {userEmail} role changed to {newRole}");
        }

        [HttpGet("system-overview")]
        public async Task<IActionResult> GetSystemOverview()
        {
            var users = await _userManager.Users.ToListAsync();
            var overview = new
            {
                TotalUsers = users.Count,
                UsersByRole = new
                {
                    SuperAdmins = users.Count(u => _userManager.IsInRoleAsync(u, UserRoles.SuperAdmin).Result),
                    Admins = users.Count(u => _userManager.IsInRoleAsync(u, UserRoles.Admin).Result),
                    Professors = users.Count(u => _userManager.IsInRoleAsync(u, UserRoles.Professor).Result),
                    Students = users.Count(u => _userManager.IsInRoleAsync(u, UserRoles.Student).Result)
                }
            };
            return Ok(overview);
        }

        // Add other super admin specific operations here
    }
} 