using Orari.Interfaces;
using Orari.Models;
using Orari.Repository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Orari.Services
{
    public class EnrollmentService : IEnrollmentService
    {
        private readonly IEnrollmentRepository _enrollmentRepository;
        public EnrollmentService(IEnrollmentRepository enrollmentRepository)
        {
            _enrollmentRepository = enrollmentRepository;
        }

        public Task<bool> EnrollStudentAsync(string studentId, int courseId)
        {
            return _enrollmentRepository.EnrollStudentAsync(studentId, courseId);
        }

        public Task<string?> GetAllEnrollmentsAsync()
        {
            return _enrollmentRepository.GetAllEnrollmentsAsync();
        }

        public async Task<IEnumerable<Students>> GetCourseStudentsAsync(int courseId)
        {
            var students = _enrollmentRepository.GetCourseStudentsAsync(courseId);
            if (students == null)
            {
                throw new Exception("No students found for this course");
            }
            return await _enrollmentRepository.GetCourseStudentsAsync(courseId);
        }

        public Task<IEnumerable<Courses>> GetStudentCoursesAsync(string studentId)
        {
            return _enrollmentRepository.GetStudentCoursesAsync(studentId);
        }

        public Task<bool> UnenrollStudentAsync(string studentId, int courseId)
        {
            return _enrollmentRepository.UnenrollStudentAsync(studentId, courseId);
        }

        public async Task<IEnumerable<Courses>> GetStudentCoursesByEmailAsync(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                throw new ArgumentException("Email cannot be empty");
            }
            return await _enrollmentRepository.GetStudentCoursesByEmailAsync(email);
        }

        public async Task<IEnumerable<Students>> GetCourseStudentsByNameAsync(string courseName)
        {
            if (string.IsNullOrEmpty(courseName))
            {
                throw new ArgumentException("Course name cannot be empty");
            }
            return await _enrollmentRepository.GetCourseStudentsByNameAsync(courseName);
        }

        public IEnumerable<Enrollments> GetEnrollmentsByStudentId(string studentId)
        {
            return _enrollmentRepository.GetEnrollmentsByStudentId(studentId);
        }
    }
}
