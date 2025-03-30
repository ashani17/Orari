using Orari.Models;

namespace Orari.Interfaces
{
    public interface IScheduleRepository
    {
        Task<IEnumerable<Schedules>> GetAllSchedules();
        Task<Schedules> GetScheduleByIdAsync(int id);
        Task<Schedules> CreateScheduleAsync(Schedules schedule);
        Task<Schedules> UpdateScheduleAsync(Schedules schedule);
        Task<bool> DeleteScheduleAsync(int id);
        Task<string?> GetSchedulesByProfesorAsync(int id);
        Task<string?> GetSchedulesByRoomAsync(int id);
    }
}
