using Orari.Interfaces;
using Orari.Models;
using System.Data;

namespace Orari.Services
{
    public class ScheduleService : IScheduleService
    {
        private readonly IScheduleRepository _scheduleRepository;
        public ScheduleService(IScheduleRepository scheduleRepository)
        {
            _scheduleRepository = scheduleRepository;
        }

        public Task<Schedules> CreateScheduleAsync(Schedules schedule)
        {
            var existingSchedule = _scheduleRepository.GetScheduleByIdAsync(schedule.SCId);
            if (existingSchedule != null)
            {
                throw new Exception("Schedule already exists");
            }
            return _scheduleRepository.CreateScheduleAsync(schedule);
        }

        public Task<bool> DeleteScheduleAsync(int id)
        {
            var existingSchedule = _scheduleRepository.GetScheduleByIdAsync(id);
            if (existingSchedule == null)
            {
                throw new Exception("Schedule not found");
            }
            return _scheduleRepository.DeleteScheduleAsync(id);
        }

        public Task<IEnumerable<Schedules>> GetAllSchedules()
        {
            return _scheduleRepository.GetAllSchedules();
        }

        public Task<Schedules> GetScheduleByIdAsync(int id)
        {
            var existingSchedule = _scheduleRepository.GetScheduleByIdAsync(id);
            if(existingSchedule ==  null)
            {
                throw new Exception("Schedule not found");
            }
            return _scheduleRepository.GetScheduleByIdAsync(id);
        }

        public Task<string?> GetSchedulesByProfesorAsync(int id)
        {
            var existingSchedule = _scheduleRepository.GetSchedulesByProfesorAsync(id);
            if (existingSchedule == null)
            {
                throw new Exception("Schedule not found");
            }
            return _scheduleRepository.GetSchedulesByProfesorAsync(id);
        }

        public Task<string?> GetSchedulesByRoomAsync(int id)
        {
            var existingSchedule = _scheduleRepository.GetSchedulesByRoomAsync(id);
            if (existingSchedule == null)
            {
                throw new Exception("Schedule not found");
            }
            return _scheduleRepository.GetSchedulesByRoomAsync(id);
        }

        public Task<Schedules> UpdateScheduleAsync(Schedules schedule)
        {
            return _scheduleRepository.UpdateScheduleAsync(schedule);
        }
    }
}
