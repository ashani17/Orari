using Orari.Interfaces;
using Orari.Models;
using Orari.Repository;

namespace Orari.Services
{
    public class ExamService : IExamService
    {
        private readonly IExamRepository _examRepository;
        private readonly IScheduleRepository _scheduleRepository;
        private readonly ICourseRepository _courseRepository;
        public readonly IRoomRepository _roomRepository;
        public ExamService(IExamRepository examRepository, IScheduleRepository scheduleRepository, ICourseRepository courseRepository, IRoomRepository roomRepository)
        {
            _examRepository = examRepository;
            _scheduleRepository = scheduleRepository;
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

            if (exam.ExamDate < DateTime.Today)
            {
                throw new Exception("Exam date cannot be in the past");
            }

            if (exam.StartTime >= exam.EndTime)
            {
                throw new Exception("Start time must be before end time");
            }

            // Validate Course
            var course = await _courseRepository.GetCourseByIdAsync(exam.CId);
            if (course == null)
            {
                throw new Exception("Course not found");
            }
            exam.CId = course.CId;
            exam.Course = course;

            // Validate Room
            var room = await _roomRepository.GetRoomByIdAsync(exam.RId);
            if (room == null)
            {
                throw new Exception("Room not found");
            }
            exam.RId = room.RId;
            exam.Room = room;

            return await _examRepository.CreateExamAsync(exam);
        }

        public async Task<bool> DeleteExamAsync(int id)
        {
            return await _examRepository.DeleteExamAsync(id);
        }

        public async Task<IEnumerable<Exams>> GetAllExamsAsync()
        {
            return await _examRepository.GetAllExamsAsync();
        }

        public async Task<Exams?> GetExamByIdAsync(int id)
        {
            return await _examRepository.GetExamByIdAsync(id);
        }

        public async Task<Exams> UpdateExamAsync(Exams exam)
            {
            return await _examRepository.UpdateExamAsync(exam);
        }

        public async Task<IEnumerable<Exams>> GetExamsByCourseAsync(int courseId)
        {
            return await _examRepository.GetExamsByCourseAsync(courseId);
        }

        public async Task<IEnumerable<Exams>> GetExamsByProfessorAsync(string professorId)
        {
            return await _examRepository.GetExamsByProfessorAsync(professorId);
        }
    }
}
