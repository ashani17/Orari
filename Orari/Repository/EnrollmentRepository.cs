using Orari.DataDbContext;
using Orari.Interfaces;
using Orari.Models;
using Microsoft.EntityFrameworkCore;

namespace Orari.Repository
{

    public class EnrollmentRepository : IEnrollmentRepository
    {
        private readonly AppDbContext _context;
        public EnrollmentRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<bool> EnrollStudentAsync(string studentId, int CId)
        {
            var student = _context.Students.FirstOrDefault(s => s.Id == studentId);
            if (student == null) return false;
            var enrollment = new Enrollments
            {
                StudentId = studentId,
                CId = CId,
                Student = student,
                Courses = _context.Courses.First(c => c.CId == CId)
            };
            _context.Enrollments.Add(enrollment);
            await _context.SaveChangesAsync();
            return true;
        }

        public Task<string?> GetAllEnrollmentsAsync()
        {
            var enrollments = _context.Enrollments
                .Include(e => e.Student)
                .Include(e => e.Courses)
                .ToList();
            if (!enrollments.Any())
            {
                return Task.FromResult<string?>(null);
            }
            // Assuming you want to return a string representation of the enrollments
            return Task.FromResult(string.Join(", ", enrollments.Select(e => $"{e.Student.SName} enrolled in {e.Courses.CName}")));
        }

        public Task<IEnumerable<Students>> GetCourseStudentsAsync(int courseId)
        {
            var students = _context.Enrollments
                .Include(e => e.Student)
                .Where(e => e.CId == courseId)
                .Select(e => e.Student)
                .ToList();
            if (!students.Any())
            {
                return Task.FromResult<IEnumerable<Students>>(new List<Students>());
            }
            return Task.FromResult<IEnumerable<Students>>(students);
        }

        public Task<IEnumerable<Courses>> GetStudentCoursesAsync(string studentId)
        {
            var courses = _context.Enrollments
                .Include(e => e.Courses)
                .Where(e => e.StudentId == studentId)
                .Select(e => e.Courses)
                .ToList();
            return Task.FromResult<IEnumerable<Courses>>(courses);
        }

        public async Task<bool> UnenrollStudentAsync(string studentId, int courseId)
        {
            var enrollment = await _context.Enrollments
                .FirstOrDefaultAsync(e => e.StudentId == studentId && e.CId == courseId);
            if (enrollment == null) return false;
            _context.Enrollments.Remove(enrollment);
            await _context.SaveChangesAsync();
            return true;
        }

        public Task<IEnumerable<Courses>> GetStudentCoursesByEmailAsync(string email)
        {
            var courses = _context.Enrollments
                .Include(e => e.Student)
                .Include(e => e.Courses)
                .Where(e => e.Student.SEmail == email)
                .Select(e => e.Courses)
                .ToList();

            if (!courses.Any())
            {
                return Task.FromResult<IEnumerable<Courses>>(new List<Courses>());
            }
            return Task.FromResult<IEnumerable<Courses>>(courses);
        }

        public Task<IEnumerable<Students>> GetCourseStudentsByNameAsync(string courseName)
        {
            var students = _context.Enrollments
                .Include(e => e.Student)
                .Include(e => e.Courses)
                .Where(e => e.Courses.CName == courseName)
                .Select(e => e.Student)
                .ToList();

            if (!students.Any())
            {
                return Task.FromResult<IEnumerable<Students>>(new List<Students>());
            }
            return Task.FromResult<IEnumerable<Students>>(students);
        }

        public IEnumerable<Enrollments> GetEnrollmentsByStudentId(string studentId)
        {
            return _context.Enrollments.Where(e => e.StudentId == studentId).ToList();
        }
    }
}
