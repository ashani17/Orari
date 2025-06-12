using Orari.DataDbContext;
using Orari.Interfaces;
using Orari.Models;

namespace Orari.Repository
{
    public class StudentRepository : IStudentRepository
    {
        private readonly AppDbContext _context;
        public StudentRepository(AppDbContext context)
        {
            _context = context;
        }
        public Task<Students> CreateStudentAsync(Students student)
        {
            _context.Students.Add(student);
            _context.SaveChanges();
            return Task.FromResult(student);
        }

        public Task<bool> DeleteStudentAsync(string id)
        {
            var student = _context.Students.Find(id);
            if (student == null) return Task.FromResult(false);
            _context.Students.Remove(student);
            _context.SaveChanges();
            return Task.FromResult(true);
        }

        public Task<IEnumerable<Students>> GetAllStudents()
        {
            var students = _context.Students.ToList();
            if (!students.Any())
            {
                return Task.FromResult<IEnumerable<Students>>(new List<Students>());
            }
            return Task.FromResult<IEnumerable<Students>>(students);
        }

        public Task<Students> GetStudentByIdAsync(string id)
        {
            var student = _context.Students.FirstOrDefault(s => s.Id == id);
            if (student == null) throw new Exception("Student not found");
            return Task.FromResult(student);
        }

        public async Task<Students> GetStudentsByEmailAsync(string email)
        {
            var student = _context.Students.FirstOrDefault(s => s.SEmail == email);
            if (student == null) return (null);
            return (student);
        }

        public Task<Students> UpdateStudentAsync(Students student)
        {
            var existingStudent = _context.Students.Find(student.Id);
            if (existingStudent == null) throw new Exception("Student not found");
            existingStudent.SName = student.SName;
            existingStudent.SSurname = student.SSurname;
            existingStudent.SPassword = student.SPassword;
            existingStudent.SEmail = student.SEmail;
            existingStudent.SCreatedAt = student.SCreatedAt;
            existingStudent.SUpdatedAt = student.SUpdatedAt;
            _context.SaveChanges();
            return Task.FromResult(existingStudent);
        }
    }
}
