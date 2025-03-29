using Orari.Interfaces;
using Orari.Models;

namespace Orari.Repository
{
    public class CourseRepository : ICourseRepository
    {
        public Task<Courses> CreateCourseAsync(Courses course)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteCourseAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Courses>> GetAllCourses()
        {
            throw new NotImplementedException();
        }

        public Task<Courses> GetCourseByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Courses> UpdateCourseAsync(Courses course)
        {
            throw new NotImplementedException();
        }
    }
}
