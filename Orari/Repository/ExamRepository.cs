using Orari.DataDbContext;
using Orari.Interfaces;
using Orari.Models;

namespace Orari.Repository
{
    public class ExamRepository : IExamRepository
    {

        private readonly AppDbContext _context;
        public ExamRepository(AppDbContext context)
        {
            _context = context;
        }
        public Task<Exams> CreateExamAsync(Exams exam)
        {
            _context.Exams.Add(exam);
            _context.SaveChanges();
            return Task.FromResult(exam);

        }

        public Task<bool> DeleteExamAsync(int id)
        {
            var exam = _context.Exams.Find(id);
            if (exam == null) return Task.FromResult(false);
            _context.Exams.Remove(exam);
            _context.SaveChanges();
            return Task.FromResult(true);

        }

        public Task<IEnumerable<Exams>> GetAllExams()
        {
            var exams = _context.Exams.ToList();
            if (!exams.Any())
            {
                return Task.FromResult<IEnumerable<Exams>>(new List<Exams>());
            }
            return Task.FromResult<IEnumerable<Exams>>(exams);

        }

        public Task<Exams> GetExamByIdAsync(int id)
        {
            var exam = _context.Exams.FirstOrDefault(e => e.EId == id);
            if (exam == null) throw new Exception("Exam not found");
            return Task.FromResult(exam);

        }

        public Task<Exams> UpdateExamAsync(Exams exam)
        {
            var existingExam = _context.Exams.Find(exam.EId);
            if (existingExam == null) throw new Exception("Exam not found");
            existingExam.ExamName = exam.ExamName;
            _context.SaveChanges();
            return Task.FromResult(existingExam);
        }
    }
}
