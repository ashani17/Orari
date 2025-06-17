using Orari.Models;

namespace Orari.Interfaces
{
    public interface IEnrollmentRepository
    {
        Task<bool> EnrollStudentAsync(string studentId, int CId);
        Task<string?> GetAllEnrollmentsAsync();
        Task<IEnumerable<User>> GetCourseStudentsAsync(int courseId);
        Task<IEnumerable<Courses>> GetStudentCoursesAsync(string studentId);
        Task<bool> UnenrollStudentAsync(string studentId, int courseId);
        Task<IEnumerable<Courses>> GetStudentCoursesByEmailAsync(string email);
        Task<IEnumerable<User>> GetCourseStudentsByNameAsync(string courseName);
        IEnumerable<Enrollments> GetEnrollmentsByStudentId(string studentId);
    }
}
