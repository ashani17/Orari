using Orari.Interfaces;
using Orari.Models;

namespace Orari.Services
{
    public class ExamService : IExamService
    {
        private readonly IExamRepository _examRepository;
        public ExamService(IExamRepository examRepository)
        {
            _examRepository = examRepository;
        }

        public Task<Exams> CreateExamAsync(Exams exam)
        {
            var existingExam = _examRepository.GetExamByIdAsync(exam.EId);
            if (existingExam != null)
            {
                throw new Exception("Exam already exists");
            }
            return _examRepository.CreateExamAsync(exam);
        }

        public Task<bool> DeleteExamAsync(int id)
        {
            var existingExam = _examRepository.GetExamByIdAsync(id);
            if (existingExam == null)
            {
                throw new Exception("Exam not found");
            }
            return _examRepository.DeleteExamAsync(id);
        }

        public Task<IEnumerable<Exams>> GetAllExams()
        {
            return _examRepository.GetAllExams();
        }

        public Task<Exams> GetExamByIdAsync(int id)
        {
            var exam = _examRepository.GetExamByIdAsync(id);
            if (exam == null)
            {
                throw new Exception("Exam not found");
            }
            return _examRepository.GetExamByIdAsync(id);
        }

        public Task<Exams> UpdateExamAsync(Exams exam)
        {
            return _examRepository.UpdateExamAsync(exam);
        }
    }
}
