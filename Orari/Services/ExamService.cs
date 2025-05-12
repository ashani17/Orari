using Orari.Interfaces;
using Orari.Models;
using Orari.Repository;

namespace Orari.Services
{
    public class ExamService : IExamService
    {
        private readonly IExamRepository _examRepository;
        private readonly IScheduleRepository _scheduleRepository;
        private readonly IProfesorRepository _professorRepository;
        private readonly ICourseRepository _courseRepository;
        public readonly IRoomRepository _roomRepository;
        public ExamService(IExamRepository examRepository, IScheduleRepository scheduleRepository, IProfesorRepository profesorRepository, ICourseRepository courseRepository, IRoomRepository roomRepository)
        {
            _examRepository = examRepository;
            _scheduleRepository = scheduleRepository;
            _professorRepository = profesorRepository;
            _courseRepository = courseRepository;
            _roomRepository = roomRepository;
        }

        public async Task<Exams> CreateExamAsync(Exams exam)
        {
            // Basic validation
            if (string.IsNullOrEmpty(exam.ExamName))
            {
                throw new Exception("Exam name is required");
            }

            // Validate Course
            var course = await _courseRepository.GetCourseByIdAsync(exam.CId);
            if (course == null)
            {
                throw new Exception("Course not found");
            }
            
            // Set both Course IDs
            exam.CId = course.CId;
            exam.Course = course;

            // Validate Professor
            var professor = await _professorRepository.GetProfesorByEmailAsync(exam.PId);
            if (professor == null)
            {
                throw new Exception("Professor not found");
            }

            // Set both Professor IDs
            exam.PId = professor.PId;
            exam.Profesor = professor;

            // Validate Room
            var room = await _roomRepository.GetRoomByIdAsync(exam.RId);
            if (room == null)
            {
                throw new Exception("Room not found");
            }

            return await _examRepository.CreateExamAsync(exam);
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
