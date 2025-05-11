using Microsoft.VisualBasic;
using Orari.Models;

namespace Orari.Interfaces
{
    public interface IScheduleService
    {
        Task<IEnumerable<Schedules>> GetAllSchedules();
        Task<Schedules> GetScheduleByIdAsync(int id);
        Task<Schedules> CreateScheduleAsync(Schedules schedule);
        Task<Schedules> GetScheduleByTimeAndDateAsync(DateOnly date, TimeOnly starttime, TimeOnly endtime);
        Task<Schedules> UpdateScheduleAsync(Schedules schedule);
        Task<bool> DeleteScheduleAsync(int id);
        Task<string?> GetSchedulesByProfesorAsync(int id);
        Task<string?> GetSchedulesByRoomAsync(int id);
    }
}
