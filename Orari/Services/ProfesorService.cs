using Orari.Interfaces;
using Orari.Models;
using Microsoft.AspNetCore.Identity;
using Orari.Constants;

namespace Orari.Services
{
    public class ProfesorService : IProfesorService
    {
        private readonly IProfesorRepository _profesorRepository;
        private readonly UserManager<IdentityUser> _userManager;
        public ProfesorService(IProfesorRepository profesorRepository, UserManager<IdentityUser> userManager)
        {
            _profesorRepository = profesorRepository;
            _userManager = userManager;
        }
        public async Task<Profesors> CreateProfesorAsync(Profesors profesor)
        {
            var existingProfesor = await _profesorRepository.GetProfesorByEmailAsync(profesor.PEmail);
            if (existingProfesor != null)
            {
                throw new Exception("Profesor already exists");
            }
            return await _profesorRepository.CreateProfesorAsync(profesor);
        }

        public async Task<bool> DeleteProfesorAsync(int id)
        {
            var existingProfesor = await _profesorRepository.GetProfesorByEmailAsync(id);
            if (existingProfesor == null)
            {
                throw new Exception("Profesor not found");
            }
            return await _profesorRepository.DeleteProfesorAsync(id);
        }

        public async Task<IEnumerable<Profesors>> GetAllProfesors()
        {
            return await _profesorRepository.GetAllProfesors();
        }

        public Task<Profesors?> GetProfesorByEmailAsync(string PEmail)
        {
            var existingProfesor = _profesorRepository.GetProfesorByEmailAsync(PEmail);
            if (existingProfesor == null)
            {
                throw new Exception("Profesor not found");
            }
            return existingProfesor;
        }

        public async Task<Profesors> GetProfesorByIdAsync(int id)
        {
            var profesor = await _profesorRepository.GetProfesorByEmailAsync(id);
            if (profesor == null)
            {
                throw new Exception("Profesor not found");
            }
            return await _profesorRepository.GetProfesorByEmailAsync(id);
        }

        public async Task<Profesors> UpdateProfesorAsync(Profesors profesor)
        {
            return await _profesorRepository.UpdateProfesorAsync(profesor);
        }

        public async Task<IEnumerable<Profesors>> GetAllAdminsAsync()
        {
            var adminUsers = await _userManager.GetUsersInRoleAsync(UserRoles.Admin);
            var adminEmails = adminUsers.Select(u => u.Email).Where(e => e != null).ToList();
            
            // Get the professor records for these admin users
            var adminProfessors = await _profesorRepository.GetProfesorsByEmailsAsync(adminEmails);
            return adminProfessors;
        }
    }
}
