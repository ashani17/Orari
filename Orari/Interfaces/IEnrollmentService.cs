using Orari.Models;

namespace Orari.Interfaces
{
    public interface IEnrollmentService
    {
        Task<bool> EnrollStudentAsync(string studentId, int courseId);
        Task<bool> UnenrollStudentAsync(string studentId, int courseId);
        Task<IEnumerable<Courses>> GetStudentCoursesAsync(string studentId);
        Task<IEnumerable<Students>> GetCourseStudentsAsync(int courseId);
        Task<string?> GetAllEnrollmentsAsync();
        Task<IEnumerable<Courses>> GetStudentCoursesByEmailAsync(string email);
        Task<IEnumerable<Students>> GetCourseStudentsByNameAsync(string courseName);
        IEnumerable<Enrollments> GetEnrollmentsByStudentId(string studentId);
    }
}
