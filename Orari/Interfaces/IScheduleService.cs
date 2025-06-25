using Microsoft.VisualBasic;
using Orari.Models;

namespace Orari.Interfaces
{
    public interface IScheduleService
    {
        Task<IEnumerable<Schedules>> GetAllSchedulesAsync();
        Task<Schedules?> GetScheduleByIdAsync(int id);
        Task<Schedules> CreateScheduleAsync(Schedules schedule);
        Task<Schedules> UpdateScheduleAsync(Schedules schedule);
        Task<bool> DeleteScheduleAsync(int id);
        Task<IEnumerable<Schedules>> GetSchedulesByCourseAsync(int courseId);
        Task<IEnumerable<Schedules>> GetSchedulesByProfessorAsync(string professorId);
        Task<IEnumerable<Schedules>> GetSchedulesByStudentAsync(string studentId);
    }
}
