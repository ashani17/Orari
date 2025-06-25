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

        public async Task<IEnumerable<Schedules>> GetAllSchedulesAsync()
        {
            return await _context.Schedules
                .Include(s => s.Course)
                .Include(s => s.Exam)
                .Include(s => s.Room)
                .ToListAsync();
        }

        public async Task<IEnumerable<Schedules>> GetAllSchedulesForUniquenessCheckAsync()
        {
            return await _context.Schedules
                .Select(s => new Schedules
                {
                    SId = s.SId,
                    Date = s.Date,
                    StartTime = s.StartTime,
                    EndTime = s.EndTime,
                    RId = s.RId,
                    ProfessorId = s.ProfessorId,
                    CId = s.CId,
                    EId = s.EId,
                    RecurringScheduleId = s.RecurringScheduleId
                })
                .ToListAsync();
        }

        public async Task<Schedules?> GetScheduleByIdAsync(int id)
        {
            return await _context.Schedules
                .Include(s => s.Course)
                .Include(s => s.Exam)
                .Include(s => s.Room)
                .FirstOrDefaultAsync(s => s.SId == id);
        }

        public async Task<Schedules> CreateScheduleAsync(Schedules schedule)
        {
            _context.Schedules.Add(schedule);
            await _context.SaveChangesAsync();
            return schedule;
        }

        public async Task<Schedules> UpdateScheduleAsync(Schedules schedule)
        {
            _context.Schedules.Update(schedule);
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

        public async Task<IEnumerable<Schedules>> GetSchedulesByCourseAsync(int courseId)
        {
            return await _context.Schedules
                .Where(s => s.CId == courseId)
                .Include(s => s.Course)
                .Include(s => s.Exam)
                .Include(s => s.Room)
                .ToListAsync();
        }

        public async Task<IEnumerable<Schedules>> GetSchedulesByProfessorAsync(string professorId)
        {
            return await _context.Schedules
                .Where(s => s.ProfessorId == professorId)
                .Include(s => s.Course)
                .Include(s => s.Exam)
                .Include(s => s.Room)
                .ToListAsync();
        }

        public async Task<IEnumerable<Schedules>> GetSchedulesByStudentAsync(string studentId)
        {
            // Get all course IDs the student is enrolled in
            var courseIds = await _context.Enrollments
                .Where(e => e.StudentId == studentId)
                .Select(e => e.CId)
                .ToListAsync();

            // Get all schedules for those courses
            return await _context.Schedules
                .Where(s => courseIds.Contains(s.CId))
                .Include(s => s.Course)
                .Include(s => s.Exam)
                .Include(s => s.Room)
                .ToListAsync();
        }
    }
}
