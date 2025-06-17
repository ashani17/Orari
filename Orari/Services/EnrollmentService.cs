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

        public async Task<bool> EnrollStudentAsync(string studentId, int CId)
        {
            return await _enrollmentRepository.EnrollStudentAsync(studentId, CId);
        }

        public async Task<string?> GetAllEnrollmentsAsync()
        {
            return await _enrollmentRepository.GetAllEnrollmentsAsync();
        }

        public async Task<IEnumerable<User>> GetCourseStudentsAsync(int courseId)
        {
            var students = await _enrollmentRepository.GetCourseStudentsAsync(courseId);
            if (students == null || !students.Any())
            {
                throw new Exception("No students found for this course");
            }
            return students;
        }

        public async Task<IEnumerable<Courses>> GetStudentCoursesAsync(string studentId)
        {
            return await _enrollmentRepository.GetStudentCoursesAsync(studentId);
        }

        public async Task<bool> UnenrollStudentAsync(string studentId, int courseId)
        {
            return await _enrollmentRepository.UnenrollStudentAsync(studentId, courseId);
        }

        public async Task<IEnumerable<Courses>> GetStudentCoursesByEmailAsync(string email)
        {
            var courses = await _enrollmentRepository.GetStudentCoursesByEmailAsync(email);
            if (courses == null || !courses.Any())
            {
                throw new Exception("No courses found for this student");
            }
            return courses;
        }

        public async Task<IEnumerable<User>> GetCourseStudentsByNameAsync(string courseName)
        {
            var students = await _enrollmentRepository.GetCourseStudentsByNameAsync(courseName);
            if (students == null || !students.Any())
            {
                throw new Exception("No students found for this course");
            }
            return students;
        }

        public IEnumerable<Enrollments> GetEnrollmentsByStudentId(string studentId)
        {
            return _enrollmentRepository.GetEnrollmentsByStudentId(studentId);
        }
    }
}
