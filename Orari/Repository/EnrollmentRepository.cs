using Orari.Interfaces;
using Orari.Models;

namespace Orari.Repository
{
    public class EnrollmentRepository : IEnrollmentRepository
    {
        public Task<bool> EnrollStudentAsync(int studentId, int courseId)
        {
            throw new NotImplementedException();
        }

        public Task<string?> GetAllEnrollmentsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Students>> GetCourseStudentsAsync(int courseId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Courses>> GetStudentCoursesAsync(int studentId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UnenrollStudentAsync(int studentId, int courseId)
        {
            throw new NotImplementedException();
        }
    }
}
