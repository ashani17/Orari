using Microsoft.EntityFrameworkCore.ChangeTracking;
using Orari.DataDbContext;
using Orari.Interfaces;
using Orari.Models;

namespace Orari.Repository
{
    public class ProfesorRepository : IProfesorRepository
    {
        private readonly AppDbContext _context;
        public ProfesorRepository(AppDbContext context)
        {
            _context = context;
        }

        public Task<Profesors> CreateProfesorAsync(Profesors profesor)
        {
            var entry = _context.Profesors.Add(profesor);
            _context.SaveChanges();
            return Task.FromResult(entry.Entity);
        }

        public Task<bool> DeleteProfesorAsync(int id)
        {
            var profesor = _context.Profesors.FirstOrDefault(p => p.PId == id);
            if (profesor == null) return Task.FromResult(false);
            _context.Profesors.Remove(profesor);
            _context.SaveChanges();
            return Task.FromResult(true);
        }

        public Task<IEnumerable<Profesors>> GetAllProfesors()
        {
            var profesors = _context.Profesors.ToList();
            if (!profesors.Any())
            {
                return Task.FromResult<IEnumerable<Profesors>>(new List<Profesors>());
            }
            return Task.FromResult<IEnumerable<Profesors>>(profesors);
        }

        public Task<Profesors?> GetProfesorByEmailAsync(string email)
        {
            var profesor = _context.Profesors.FirstOrDefault(p => p.PEmail == email);
            if (profesor == null) return Task.FromResult<Profesors?>(null);
            return Task.FromResult<Profesors?>(profesor);
        }

        public Task<Profesors> GetProfesorByEmailAsync(int id)
        {
            var profesor = _context.Profesors.FirstOrDefault(p => p.PId == id);
            if (profesor == null) throw new Exception("Profesor not found");
            return Task.FromResult(profesor);
        }

        public Task<Profesors> UpdateProfesorAsync(Profesors profesor)
        {
            var existingProfesor = _context.Profesors.FirstOrDefault(p => p.PId == profesor.PId);
            if (existingProfesor == null) throw new Exception("Profesor not found");
            existingProfesor.PName = profesor.PName;
            existingProfesor.PEmail = profesor.PEmail;
            existingProfesor.PPhone = profesor.PPhone;
            existingProfesor.PSurname = profesor.PSurname;
            existingProfesor.PSubject = profesor.PSubject;
            _context.SaveChanges();
            return Task.FromResult(existingProfesor);
        }
    }
}
