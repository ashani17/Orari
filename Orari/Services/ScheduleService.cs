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

        public async Task<IEnumerable<Schedules>> GetAllSchedulesAsync()
        {
            return await _scheduleRepository.GetAllSchedulesAsync();
        }

        public async Task<Schedules?> GetScheduleByIdAsync(int id)
        {
            return await _scheduleRepository.GetScheduleByIdAsync(id);
        }

        public async Task<Schedules> CreateScheduleAsync(Schedules schedule)
        {
            // Check if schedule already exists for the same time and date
            var existingSchedules = await _scheduleRepository.GetAllSchedulesAsync();
            var conflictingSchedule = existingSchedules.FirstOrDefault(s => 
                s.Date == schedule.Date && 
                s.StartTime == schedule.StartTime && 
                s.EndTime == schedule.EndTime);

            if (conflictingSchedule != null)
            {
                throw new Exception("Schedule already exists");
            }

            return await _scheduleRepository.CreateScheduleAsync(schedule);
        }

        public async Task<Schedules> UpdateScheduleAsync(Schedules schedule)
        {
            return await _scheduleRepository.UpdateScheduleAsync(schedule);
        }

        public async Task<bool> DeleteScheduleAsync(int id)
        {
            return await _scheduleRepository.DeleteScheduleAsync(id);
        }

        public async Task<IEnumerable<Schedules>> GetSchedulesByCourseAsync(int courseId)
        {
            return await _scheduleRepository.GetSchedulesByCourseAsync(courseId);
        }

        public async Task<IEnumerable<Schedules>> GetSchedulesByProfessorAsync(string professorId)
        {
            return await _scheduleRepository.GetSchedulesByProfessorAsync(professorId);
        }
    }
}
