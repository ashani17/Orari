using Orari.Interfaces;
using Orari.Models;

namespace Orari.Services
{
    public class EnrollmentService : IEnrollmentService
    {
        private readonly IEnrollmentRepository _enrollmentRepository;
        public EnrollmentService(IEnrollmentRepository enrollmentRepository)
        {
            _enrollmentRepository = enrollmentRepository;
        }

        public async Task<bool> EnrollStudentAsync(int studentId, int courseId)
        {
            var existingEnrollments = await _enrollmentRepository.GetStudentCoursesAsync(studentId);
            if (existingEnrollments.Any(course => course.CId == courseId))
            {
                throw new Exception("Student is already enrolled in this course");
            }
            return await _enrollmentRepository.EnrollStudentAsync(studentId, courseId);
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

        public Task<IEnumerable<Courses>> GetStudentCoursesAsync(int studentId)
        {
            var courses = _enrollmentRepository.GetStudentCoursesAsync(studentId);
            if (courses == null)
            {
                throw new Exception("No courses found for this student");
            }
            return _enrollmentRepository.GetStudentCoursesAsync(studentId);
        }

        public async Task<bool> UnenrollStudentAsync(int studentId, int courseId)
        {
            var existingEnrollments = await _enrollmentRepository.GetStudentCoursesAsync(studentId);
            if (!existingEnrollments.Any(course => course.CId == courseId))
            {
                throw new Exception("Student is not enrolled in this course");
            }
            return await _enrollmentRepository.UnenrollStudentAsync(studentId, courseId);
        }
    }
}
