using Orari.Models;

namespace Orari.Interfaces
{
    public interface IProfesorRepository
    {
        Task<IEnumerable<Profesors>> GetAllProfesors();
        Task<Profesors> GetProfesorByEmailAsync(int id);
        Task<Profesors?> GetProfesorByEmailAsync(string email);
        Task<Profesors> CreateProfesorAsync(Profesors profesor);
        Task<Profesors> UpdateProfesorAsync(Profesors profesor);
        Task<bool> DeleteProfesorAsync(int id);
        Task<IEnumerable<Profesors>> GetProfesorsByEmailsAsync(IEnumerable<string> emails);
    }
}
