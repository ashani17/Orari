using Orari.Models;

namespace Orari.Interfaces
{
    public interface IEnrollmentRepository
    {
        Task<bool> EnrollStudentAsync(int studentId, int courseId);
        Task<bool> UnenrollStudentAsync(int studentId, int courseId);
        Task<IEnumerable<Courses>> GetStudentCoursesAsync(int studentId);
        Task<IEnumerable<Students>> GetCourseStudentsAsync(int courseId);
        Task<string?> GetAllEnrollmentsAsync();
    }
}
