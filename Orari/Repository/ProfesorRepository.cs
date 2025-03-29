using Orari.Interfaces;
using Orari.Models;

namespace Orari.Repository
{
    public class ProfesorRepository : IProfesorRepository
    {
        public Task<Profesors> CreateProfesorAsync(Profesors profesor)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteProfesorAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Profesors>> GetAllProfesors()
        {
            throw new NotImplementedException();
        }

        public Task<Profesors> GetProfesorByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Profesors> UpdateProfesorAsync(Profesors profesor)
        {
            throw new NotImplementedException();
        }
    }
}
