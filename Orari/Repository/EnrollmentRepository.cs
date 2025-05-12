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
        public Task<bool> EnrollStudentAsync(int SId, int CId)
        {
            var student = _context.Students.FirstOrDefault(s => s.SId == SId);
            var course = _context.Courses.FirstOrDefault(c => c.CId == CId);

            if (student == null || course == null)
            {
                return Task.FromResult(false);
            }

            var enrollment = new Enrollments
            {
                SId = SId,
                CId = CId,
                Students = student,
                Courses = course
            };
            _context.Enrollments.Add(enrollment);
            return _context.SaveChangesAsync().ContinueWith(t => t.Result > 0);
        }

        public Task<string?> GetAllEnrollmentsAsync()
        {
            var enrollments = _context.Enrollments
                .Include(e => e.Students)
                .Include(e => e.Courses)
                .ToList();
            if (!enrollments.Any())
            {
                return Task.FromResult<string?>(null);
            }
            // Assuming you want to return a string representation of the enrollments
            return Task.FromResult(string.Join(", ", enrollments.Select(e => $"{e.Students.SName} enrolled in {e.Courses.CName}")));
        }

        public Task<IEnumerable<Students>> GetCourseStudentsAsync(int courseId)
        {
            var students = _context.Enrollments
                .Include(e => e.Students)
                .Where(e => e.CId == courseId)
                .Select(e => e.Students)
                .ToList();
            if (!students.Any())
            {
                return Task.FromResult<IEnumerable<Students>>(new List<Students>());
            }
            return Task.FromResult<IEnumerable<Students>>(students);
        }

        public Task<IEnumerable<Courses>> GetStudentCoursesAsync(int studentId)
        {
            var courses = _context.Enrollments
                .Include(e => e.Courses)
                .Where(e => e.SId == studentId)
                .Select(e => e.Courses)
                .ToList();
            if (!courses.Any())
            {
                return Task.FromResult<IEnumerable<Courses>>(new List<Courses>());
            }
            return Task.FromResult<IEnumerable<Courses>>(courses);
        }

        public Task<bool> UnenrollStudentAsync(int studentId, int courseId)
        {
            var enrollment = _context.Enrollments
                .FirstOrDefault(e => e.SId == studentId && e.CId == courseId);
            if (enrollment == null)
            {
                return Task.FromResult(false);
            }
            _context.Enrollments.Remove(enrollment);
            return _context.SaveChangesAsync().ContinueWith(t => t.Result > 0);
        }

        public Task<IEnumerable<Courses>> GetStudentCoursesByEmailAsync(string email)
        {
            var courses = _context.Enrollments
                .Include(e => e.Students)
                .Include(e => e.Courses)
                .Where(e => e.Students.SEmail == email)
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
                .Include(e => e.Students)
                .Include(e => e.Courses)
                .Where(e => e.Courses.CName == courseName)
                .Select(e => e.Students)
                .ToList();

            if (!students.Any())
            {
                return Task.FromResult<IEnumerable<Students>>(new List<Students>());
            }
            return Task.FromResult<IEnumerable<Students>>(students);
        }
    }
}
