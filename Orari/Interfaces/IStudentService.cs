using Orari.Models;

namespace Orari.Interfaces
{
    public interface IStudentService
    {
        Task<IEnumerable<Students>> GetAllStudents();
        Task<Students> GetStudentByIdAsync(string id);
        Task<Students> GetStudentsByEmailAsync(string email);
        Task<Students> CreateStudentAsync(Students student);
        Task<Students> UpdateStudentAsync(Students student);
        Task<bool> DeleteStudentAsync(string id);
    }
}
