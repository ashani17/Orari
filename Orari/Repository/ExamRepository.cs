using Orari.DataDbContext;
using Orari.Interfaces;
using Orari.Models;
using Microsoft.EntityFrameworkCore;

namespace Orari.Repository
{
    public class ExamRepository : IExamRepository
    {
        private readonly AppDbContext _context;

        public ExamRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Exams>> GetAllExamsAsync()
        {
            return await _context.Exams
                .Include(e => e.Course)
                .Include(e => e.Schedule)
                .Include(e => e.Professor)
                .Include(e => e.Room)
                .ToListAsync();
        }

        public async Task<Exams?> GetExamByIdAsync(int id)
        {
            return await _context.Exams
                .Include(e => e.Course)
                .Include(e => e.Schedule)
                .Include(e => e.Professor)
                .Include(e => e.Room)
                .FirstOrDefaultAsync(e => e.EId == id);
        }

        public async Task<Exams> CreateExamAsync(Exams exam)
        {
            _context.Exams.Add(exam);
            await _context.SaveChangesAsync();
            return exam;
        }

        public async Task<Exams> UpdateExamAsync(Exams exam)
        {
            _context.Exams.Update(exam);
            await _context.SaveChangesAsync();
            return exam;
        }

        public async Task<bool> DeleteExamAsync(int id)
        {
            var exam = await _context.Exams.FindAsync(id);
            if (exam == null) return false;
            
            _context.Exams.Remove(exam);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Exams>> GetExamsByCourseAsync(int courseId)
        {
            return await _context.Exams
                .Where(e => e.CId == courseId)
                .Include(e => e.Course)
                .Include(e => e.Schedule)
                .Include(e => e.Professor)
                .Include(e => e.Room)
                .ToListAsync();
        }

        public async Task<IEnumerable<Exams>> GetExamsByProfessorAsync(string professorId)
        {
            return await _context.Exams
                .Where(e => e.ProfessorId == professorId)
                .Include(e => e.Course)
                .Include(e => e.Schedule)
                .Include(e => e.Professor)
                .Include(e => e.Room)
                .ToListAsync();
        }

        public Task<Exams> GetExamByNameAsync(string name)
        {
            var exam = _context.Exams.FirstOrDefault(e => e.ExamName == name);
            if (exam == null) throw new Exception("Exam not found");
            return Task.FromResult(exam);
        }
    }
}
