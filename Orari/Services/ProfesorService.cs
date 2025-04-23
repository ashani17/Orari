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
        public Task<Profesors> CreateProfesorAsync(Profesors profesor)
        {
            var existingProfesor = _profesorRepository.GetProfesorByIdAsync(profesor.PId);
            if (existingProfesor != null)
            {
                throw new Exception("Profesor already exists");
            }
            return _profesorRepository.CreateProfesorAsync(profesor);
        }

        public Task<bool> DeleteProfesorAsync(int id)
        {
            var existingProfesor = _profesorRepository.GetProfesorByIdAsync(id);
            if (existingProfesor == null)
            {
                throw new Exception("Profesor not found");
            }
            return _profesorRepository.DeleteProfesorAsync(id);
        }

        public Task<IEnumerable<Profesors>> GetAllProfesors()
        {
            return _profesorRepository.GetAllProfesors();
        }

        public Task<Profesors> GetProfesorByIdAsync(int id)
        {
            var profesor = _profesorRepository.GetProfesorByIdAsync(id);
            if (profesor == null)
            {
                throw new Exception("Profesor not found");
            }
            return _profesorRepository.GetProfesorByIdAsync(id);
        }

        public Task<Profesors> UpdateProfesorAsync(Profesors profesor)
        {
            return _profesorRepository.UpdateProfesorAsync(profesor);
        }
    }
}
