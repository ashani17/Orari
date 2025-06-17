using Orari.DataDbContext;
using Orari.Interfaces;
using Orari.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace Orari.Repository
{
    public class EnrollmentRepository : IEnrollmentRepository
    {
        private readonly AppDbContext _context;
        private readonly UserManager<User> _userManager;

        public EnrollmentRepository(AppDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<bool> EnrollStudentAsync(string studentId, int CId)
        {
            // Check if the user exists and has Student role
            var user = await _userManager.FindByIdAsync(studentId);
            if (user == null) return false;
            
            var roles = await _userManager.GetRolesAsync(user);
            if (!roles.Contains("Student")) return false;

            var enrollment = new Enrollments
            {
                StudentId = studentId,
                CId = CId,
                Student = user,
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
            return Task.FromResult(string.Join(", ", enrollments.Select(e => $"{e.Student.FirstName} {e.Student.LastName} enrolled in {e.Courses.CName}")));
        }

        public async Task<IEnumerable<User>> GetCourseStudentsAsync(int courseId)
        {
            var studentIds = _context.Enrollments
                .Where(e => e.CId == courseId)
                .Select(e => e.StudentId)
                .ToList();

            var students = new List<User>();
            foreach (var studentId in studentIds)
            {
                var user = await _userManager.FindByIdAsync(studentId);
                if (user != null)
                {
                    var roles = await _userManager.GetRolesAsync(user);
                    if (roles.Contains("Student"))
                    {
                        students.Add(user);
                    }
                }
            }

            return students;
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

        public async Task<IEnumerable<Courses>> GetStudentCoursesByEmailAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null) return new List<Courses>();

            var courses = _context.Enrollments
                .Include(e => e.Courses)
                .Where(e => e.StudentId == user.Id)
                .Select(e => e.Courses)
                .ToList();

            return courses;
        }

        public async Task<IEnumerable<User>> GetCourseStudentsByNameAsync(string courseName)
        {
            var studentIds = _context.Enrollments
                .Include(e => e.Courses)
                .Where(e => e.Courses.CName == courseName)
                .Select(e => e.StudentId)
                .ToList();

            var students = new List<User>();
            foreach (var studentId in studentIds)
            {
                var user = await _userManager.FindByIdAsync(studentId);
                if (user != null)
                {
                    var roles = await _userManager.GetRolesAsync(user);
                    if (roles.Contains("Student"))
                    {
                        students.Add(user);
                    }
                }
            }

            return students;
        }

        public IEnumerable<Enrollments> GetEnrollmentsByStudentId(string studentId)
        {
            return _context.Enrollments.Where(e => e.StudentId == studentId).ToList();
        }
    }
}
