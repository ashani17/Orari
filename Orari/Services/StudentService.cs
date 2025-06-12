using Microsoft.EntityFrameworkCore;
using Orari.Interfaces;
using Orari.Models;

namespace Orari.Services
{
    public class StudentService : IStudentService
    {
        private readonly IStudentRepository _studentRepository;
        public StudentService(IStudentRepository studentRepository)
        {
            _studentRepository = studentRepository;
        }
        public async Task<IEnumerable<Students>> GetAllStudents()
        {
            return await _studentRepository.GetAllStudents();
        }
        public Task<Students> GetStudentByIdAsync(string id)
        {
            return _studentRepository.GetStudentByIdAsync(id);
        }
        public async Task<Students> CreateStudentAsync(Students student)
        {
            var existingStudent = await _studentRepository.GetStudentsByEmailAsync(student.SEmail);
            if (existingStudent != null)
            {
                throw new Exception("Student already exists");
            }
            return await _studentRepository.CreateStudentAsync(student);
        }
        public async Task<Students> UpdateStudentAsync(Students student)
        {
            return await _studentRepository.UpdateStudentAsync(student);
        }
        public async Task<bool> DeleteStudentAsync(string id)
        {
            var existingStudent = await _studentRepository.GetStudentByIdAsync(id);
            if (existingStudent == null) return false;
            return await _studentRepository.DeleteStudentAsync(id);
        }

        public async Task<Students> GetStudentsByEmailAsync(string email)
        {
            var student = await _studentRepository.GetStudentsByEmailAsync(email);
            if (student == null)
            {
                throw new Exception("Student not found");
            }
            return student;
        }
    }
}
