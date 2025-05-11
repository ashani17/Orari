using Orari.Models;

namespace Orari.Interfaces
{
    public interface IStudentRepository
    {
        Task<IEnumerable<Students>> GetAllStudents();
        Task<Students> GetStudentByIdAsync(int id);
        Task<Students> GetStudentsByEmailAsync(string email);
        Task<Students> CreateStudentAsync(Students student);
        Task<Students> UpdateStudentAsync(Students student);
        Task<bool> DeleteStudentAsync(int id);
    }
}
