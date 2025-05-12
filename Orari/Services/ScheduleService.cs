using Orari.Interfaces;
using Orari.Models;
using System.Data;

namespace Orari.Services
{
    public class ScheduleService : IScheduleService
    {
        private readonly IScheduleRepository _scheduleRepository;
        private readonly IRoomRepository _roomRepository;
        private readonly IProfesorRepository _profesorRepository;
        private readonly ICourseRepository _courseRepository;
        private readonly IExamRepository _examRepository;

        public ScheduleService(IScheduleRepository scheduleRepository, IRoomRepository roomRepository, IProfesorRepository profesorRepository, ICourseRepository courseRepository, IExamRepository examRepository)
        {
            _scheduleRepository = scheduleRepository;
            _roomRepository = roomRepository;
            _profesorRepository = profesorRepository;
            _courseRepository = courseRepository;
            _examRepository = examRepository;
        }

        public async Task<Schedules> CreateScheduleAsync(Schedules schedule)
        {
            // Check for existing schedule
            var existingSchedule = await _scheduleRepository.GetScheduleByDateAndTimeAsync(
                schedule.Date, schedule.StartTime, schedule.EndTime);
            if (existingSchedule != null)
            {
                throw new Exception("Schedule already exists");
            }

            // Validate Room
            var room = await _roomRepository.GetRoomByIdAsync(schedule.RId);
            if (room == null)
            {
                throw new Exception("Room not found");
            }

            // Validate Professor
            var professor = await _profesorRepository.GetProfesorByEmailAsync(schedule.PId);
            if (professor == null)
            {
                throw new Exception("Professor not found");
            }

            // Validate Course
            var course = await _courseRepository.GetCourseByIdAsync(schedule.CId);
            if (course == null)
            {
                throw new Exception("Course not found");
            }

            // Only validate exam if one is specified
            if (schedule.EId.HasValue)
            {
                var exam = await _examRepository.GetExamByIdAsync(schedule.EId.Value);
                if (exam == null)
                {
                    throw new Exception("Exam not found");
                }
            }

            return await _scheduleRepository.CreateScheduleAsync(schedule);
        }

        public async Task<bool> DeleteScheduleAsync(int id)
        {
            var existingSchedule = await _scheduleRepository.GetScheduleByIdAsync(id);
            if (existingSchedule == null)
            {
                throw new Exception("Schedule not found");
            }
            return await _scheduleRepository.DeleteScheduleAsync(id);
        }

        public Task<IEnumerable<Schedules>> GetAllSchedules()
        {
            return _scheduleRepository.GetAllSchedules();
        }

        public async Task<Schedules> GetScheduleByIdAsync(int id)
        {
            var existingSchedule = await _scheduleRepository.GetScheduleByIdAsync(id);
            if (existingSchedule == null)
            {
                throw new Exception("Schedule not found");
            }
            return existingSchedule;
        }

        public Task<Schedules> GetScheduleByTimeAndDateAsync(DateOnly date, TimeOnly starttime, TimeOnly endtime)
        {
            return _scheduleRepository.GetScheduleByDateAndTimeAsync(date, starttime, endtime);
        }

        public async Task<string?> GetSchedulesByProfesorAsync(int id)
        {
            var existingSchedule = await _scheduleRepository.GetSchedulesByProfesorAsync(id);
            if (existingSchedule == null)
            {
                throw new Exception("Schedule not found");
            }
            return existingSchedule;
        }

        public async Task<string?> GetSchedulesByRoomAsync(int id)
        {
            var existingSchedule = await _scheduleRepository.GetSchedulesByRoomAsync(id);
            if (existingSchedule == null)
            {
                throw new Exception("Schedule not found");
            }
            return existingSchedule;
        }

        public Task<Schedules> UpdateScheduleAsync(Schedules schedule)
        {
            return _scheduleRepository.UpdateScheduleAsync(schedule);
        }
    }
}
