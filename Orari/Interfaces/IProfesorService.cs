using Orari.Models;

namespace Orari.Interfaces
{
    public interface IProfesorService
    {
        Task<IEnumerable<Profesors>> GetAllProfesors();
        Task<Profesors> GetProfesorByIdAsync(int id);
        Task<Profesors?> GetProfesorByEmailAsync(string PEmail);
        Task<Profesors> CreateProfesorAsync(Profesors profesor);
        Task<Profesors> UpdateProfesorAsync(Profesors profesor);
        Task<bool> DeleteProfesorAsync(int id);
        Task<IEnumerable<Profesors>> GetAllAdminsAsync();
    }
}
