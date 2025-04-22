using Orari.Models;

namespace Orari.Interfaces
{
    public interface ICourseService
    {
        Task<IEnumerable<Courses>> GetAllCourses();
        Task<Courses> GetCourseByIdAsync(int id);
        Task<Courses?> GetCourseByNameAsync(string CName);
        Task<Courses> CreateCourseAsync(Courses course, string CName);
        Task<bool> UpdateCourseAsync(int id, Courses course);
        Task<bool> DeleteCourseAsync(int id);
    }
}
