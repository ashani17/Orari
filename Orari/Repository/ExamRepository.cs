using Orari.Interfaces;
using Orari.Models;

namespace Orari.Repository
{
    public class ExamRepository : IExamRepository
    {
        public Task<Exams> CreateExamAsync(Exams exam)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteExamAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Exams>> GetAllExams()
        {
            throw new NotImplementedException();
        }

        public Task<Exams> GetExamByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Exams> UpdateExamAsync(Exams exam)
        {
            throw new NotImplementedException();
        }
    }
}
