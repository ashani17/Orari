using Orari.Interfaces;
using Orari.Models;

namespace Orari.Services
{
    public class ProfesorService : IProfesorService
    {
        private readonly IProfesorRepository _profesorRepository;
        public ProfesorService(IProfesorRepository profesorRepository)
        {
            _profesorRepository = profesorRepository;
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
    }
}
