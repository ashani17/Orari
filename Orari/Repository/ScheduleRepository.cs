using Orari.Interfaces;
using Orari.Models;

namespace Orari.Repository
{
    public class ScheduleRepository : IScheduleRepository
    {
        public Task<Schedules> CreateScheduleAsync(Schedules schedule)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteScheduleAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Schedules>> GetAllSchedules()
        {
            throw new NotImplementedException();
        }

        public Task<Schedules> GetScheduleByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Schedules> UpdateScheduleAsync(Schedules schedule)
        {
            throw new NotImplementedException();
        }
    }
}
