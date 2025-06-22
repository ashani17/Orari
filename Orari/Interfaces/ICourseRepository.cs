using Orari.Models;

namespace Orari.Interfaces
{
    public interface ICourseRepository
    {
        Task<IEnumerable<Courses>> GetAllCoursesAsync();
        Task<Courses?> GetCourseByIdAsync(int id);
        Task<Courses?> GetCourseByNameAsync(string CName);
        Task<Courses> CreateCourseAsync(Courses course);
        Task<Courses> UpdateCourseAsync(Courses course);
        Task<bool> DeleteCourseAsync(int id);
        Task<IEnumerable<Courses>> GetCoursesByProfessorAsync(string professorId);
    }
}
