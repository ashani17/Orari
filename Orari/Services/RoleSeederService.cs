using Microsoft.AspNetCore.Identity;
using Orari.Constants;
using Orari.Models;

namespace Orari.Services
{
    public class RoleSeederService
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IConfiguration _configuration;

        public RoleSeederService(
            RoleManager<IdentityRole> roleManager,
            UserManager<IdentityUser> userManager,
            IConfiguration configuration)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _configuration = configuration;
        }

        public async Task SeedRolesAsync()
        {
            // Create roles if they don't exist
            foreach (var role in UserRoles.AllRoles)
            {
                if (!await _roleManager.RoleExistsAsync(role))
                {
                    await _roleManager.CreateAsync(new IdentityRole(role));
                }
            }
        }

        public async Task SeedAdminUserAsync()
        {
            var adminConfig = _configuration.GetSection("DefaultAdmin");
            var adminEmail = adminConfig["Email"];
            var adminPassword = adminConfig["Password"];
            var adminName = adminConfig["Name"];
            var adminSurname = adminConfig["Surname"];

            if (string.IsNullOrEmpty(adminEmail) || string.IsNullOrEmpty(adminPassword))
            {
                throw new InvalidOperationException("Default admin credentials not configured");
            }

            // Check if admin user exists
            var adminUser = await _userManager.FindByEmailAsync(adminEmail);
            if (adminUser == null)
            {
                // Create the admin user
                adminUser = new IdentityUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true
                };

                var result = await _userManager.CreateAsync(adminUser, adminPassword);
                if (result.Succeeded)
                {
                    // Assign admin role
                    await _userManager.AddToRoleAsync(adminUser, UserRoles.Admin);

                    // Create admin profile if needed
                    // You might want to create an admin profile in your database here
                }
                else
                {
                    var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                    throw new InvalidOperationException($"Failed to create admin user: {errors}");
                }
            }
            else
            {
                // Ensure existing user has admin role
                if (!await _userManager.IsInRoleAsync(adminUser, UserRoles.Admin))
                {
                    await _userManager.AddToRoleAsync(adminUser, UserRoles.Admin);
                }
            }
        }

        public async Task SeedSuperAdminAsync()
        {
            // First, ensure the SuperAdmin role exists
            if (!await _roleManager.RoleExistsAsync(UserRoles.SuperAdmin))
            {
                await _roleManager.CreateAsync(new IdentityRole(UserRoles.SuperAdmin));
                Console.WriteLine("SuperAdmin role created");
            }

            var superAdminConfig = _configuration.GetSection("SuperAdminCredentials");
            var email = superAdminConfig["Email"];
            var password = superAdminConfig["Password"];

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                Console.WriteLine("SuperAdmin credentials not found in configuration");
                return;
            }

            // Check if SuperAdmin exists
            var superAdmin = await _userManager.FindByEmailAsync(email);
            if (superAdmin == null)
            {
                superAdmin = new IdentityUser
                {
                    UserName = email,
                    Email = email,
                    EmailConfirmed = true
                };

                var result = await _userManager.CreateAsync(superAdmin, password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(superAdmin, UserRoles.SuperAdmin);
                    Console.WriteLine($"SuperAdmin created with email: {email}");
                }
                else
                {
                    Console.WriteLine($"Failed to create SuperAdmin: {string.Join(", ", result.Errors.Select(e => e.Description))}");
                }
            }
            else
            {
                // Ensure existing user has SuperAdmin role
                if (!await _userManager.IsInRoleAsync(superAdmin, UserRoles.SuperAdmin))
                {
                    await _userManager.AddToRoleAsync(superAdmin, UserRoles.SuperAdmin);
                    Console.WriteLine("Added SuperAdmin role to existing user");
                }
            }
        }
    }
} 