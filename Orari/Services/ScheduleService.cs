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
            // Log the incoming schedule
            Console.WriteLine($"Incoming: {schedule.Date} {schedule.StartTime}-{schedule.EndTime} Room:{schedule.RId} Course:{schedule.CId} Prof:{schedule.ProfessorId}");

            var existingSchedules = await _scheduleRepository.GetAllSchedulesForUniquenessCheckAsync();
            foreach (var s in existingSchedules)
            {
                Console.WriteLine($"Existing: {s.Date} {s.StartTime}-{s.EndTime} Room:{s.RId} Course:{s.CId} Prof:{s.ProfessorId}");
            }
            // Check if schedule already exists for the same time, date, room, course, and professor
            var conflictingSchedule = existingSchedules.FirstOrDefault(s => 
                s.Date == schedule.Date && 
                s.StartTime == schedule.StartTime && 
                s.EndTime == schedule.EndTime &&
                s.RId == schedule.RId &&
                s.CId == schedule.CId &&
                s.ProfessorId == schedule.ProfessorId
            );

            if (conflictingSchedule != null)
            {
                Console.WriteLine($"CONFLICT: {conflictingSchedule.Date} {conflictingSchedule.StartTime}-{conflictingSchedule.EndTime} Room:{conflictingSchedule.RId} Course:{conflictingSchedule.CId} Prof:{conflictingSchedule.ProfessorId}");
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

        public async Task<IEnumerable<Schedules>> GetSchedulesByStudentAsync(string studentId)
        {
            return await _scheduleRepository.GetSchedulesByStudentAsync(studentId);
        }
    }
}
