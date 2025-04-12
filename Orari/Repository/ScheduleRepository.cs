using Microsoft.EntityFrameworkCore;
using Orari.DataDbContext;
using Orari.Interfaces;
using Orari.Models;

namespace Orari.Repository
{
    public class ScheduleRepository : IScheduleRepository
    {

        private readonly AppDbContext _context;
        public ScheduleRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<Schedules> CreateScheduleAsync(Schedules schedule)
        {
            await _context.Schedules.AddAsync(schedule);
            await _context.SaveChangesAsync();
            return schedule;

        }

        public async Task<bool> DeleteScheduleAsync(int id)
        {
            var schedule = await _context.Schedules.FindAsync(id);
            if (schedule == null) return false;
            _context.Schedules.Remove(schedule);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Schedules>> GetAllSchedules()
        {
            return await Task.FromResult(_context.Schedules.ToList());
        }

        public async Task<Schedules> GetScheduleByIdAsync(int id)
        {
            var schedule = await _context.Schedules.FirstOrDefaultAsync(s => s.SCId == id);
            if (schedule == null) throw new Exception("Schedule not found");
            return schedule;

        }

        public async Task<string?> GetSchedulesByProfesorAsync(int id)
        {
            var schedules = await _context.Schedules
                .Where(s => s.PId == id)
                .ToListAsync();

            if (!schedules.Any())
            {
                return null;
            }

            // Assuming you want to return a string representation of the schedules
            return string.Join(", ", schedules.Select(s => $"{s.Date} {s.StartTime}-{s.EndTime}"));
        }

        public async Task<string?> GetSchedulesByRoomAsync(int id)
        {
            var schedules = await _context.Schedules
                .Where(s => s.RId == id)
                .ToListAsync();

            if (!schedules.Any())
            {
                return null;
            }

            // Assuming you want to return a string representation of the schedules
            return string.Join(", ", schedules.Select(s => $"{s.Date} {s.StartTime}-{s.EndTime}"));
        }

        public async Task<Schedules> UpdateScheduleAsync(Schedules schedule)
        {
            var existingSchedule = await _context.Schedules.FindAsync(schedule.SCId);
            if (existingSchedule == null)
            {
                throw new Exception("Schedule not found");
            }
            existingSchedule.Date = schedule.Date;
            _context.Entry(existingSchedule).CurrentValues.SetValues(schedule);
            await _context.SaveChangesAsync();
            return existingSchedule;
        }
    }
}
