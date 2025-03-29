using Orari.Models;

namespace Orari.Interfaces
{
    public interface IProfesorRepository
    {
        Task<IEnumerable<Profesors>> GetAllProfesors();
        Task<Profesors> GetProfesorByIdAsync(int id);
        Task<Profesors> CreateProfesorAsync(Profesors profesor);
        Task<Profesors> UpdateProfesorAsync(Profesors profesor);
        Task<bool> DeleteProfesorAsync(int id);
    }
}
