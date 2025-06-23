using Orari.Models;

namespace Orari.Interfaces
{
    public interface IScheduleRepository
    {
        Task<IEnumerable<Schedules>> GetAllSchedulesAsync();
        Task<IEnumerable<Schedules>> GetAllSchedulesForUniquenessCheckAsync();
        Task<Schedules?> GetScheduleByIdAsync(int id);
        Task<Schedules> CreateScheduleAsync(Schedules schedule);
        Task<Schedules> UpdateScheduleAsync(Schedules schedule);
        Task<bool> DeleteScheduleAsync(int id);
        Task<IEnumerable<Schedules>> GetSchedulesByCourseAsync(int courseId);
        Task<IEnumerable<Schedules>> GetSchedulesByProfessorAsync(string professorId);
        Task<IEnumerable<Schedules>> GetSchedulesByStudentAsync(string studentId);
    }
}
