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
        public async Task<Students> GetStudentByIdAsync(int id)
        {
            var student = await _studentRepository.GetStudentByIdAsync(id);
            if (student == null)
            {
                throw new Exception("Student not found");
            }
            return await _studentRepository.GetStudentByIdAsync(id);
        }
        public async Task<Students> CreateStudentAsync(Students student)
        {
            var existingStudent = await _studentRepository.GetStudentByIdAsync(student.SId);
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
        public async Task<bool> DeleteStudentAsync(int id)
        {
            var existingStudent = await _studentRepository.GetStudentByIdAsync(id);
            if (existingStudent == null)
            {
                throw new Exception("Student not found");
            }
            return await _studentRepository.DeleteStudentAsync(id);
        }
    }
}
