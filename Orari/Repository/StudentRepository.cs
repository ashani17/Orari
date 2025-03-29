using Orari.Interfaces;
using Orari.Models;

namespace Orari.Repository
{
    public class StudentRepository : IStudentRepository
    {
        public Task<Students> CreateStudentAsync(Students student)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteStudentAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Students>> GetAllStudents()
        {
            throw new NotImplementedException();
        }

        public Task<Students> GetStudentByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Students> UpdateStudentAsync(Students student)
        {
            throw new NotImplementedException();
        }
    }
}
