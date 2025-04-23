using Orari.Models;

namespace Orari.Interfaces
{
    public interface IStudentService
    {
        Task<IEnumerable<Students>> GetAllStudents();
        Task<Students> GetStudentByIdAsync(int id);
        Task<Students> CreateStudentAsync(Students student);
        Task<Students> UpdateStudentAsync(Students student);
        Task<bool> DeleteStudentAsync(int id);
    }
}
