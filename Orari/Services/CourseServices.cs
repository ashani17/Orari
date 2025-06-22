using Orari.Interfaces;
using Orari.Models;

namespace Orari.Services
{
    public class CourseServices : ICourseService
    {
        private readonly ICourseRepository _courseRepository;

        public CourseServices(ICourseRepository courseRepository)
        {
            _courseRepository = courseRepository;
        }

        public async Task<IEnumerable<Courses>> GetAllCoursesAsync()
        {
            return await _courseRepository.GetAllCoursesAsync();
        }

        public async Task<Courses?> GetCourseByIdAsync(int id)
        {
            return await _courseRepository.GetCourseByIdAsync(id);
        }

        public async Task<Courses> GetCourseByNameAsync(string CName)
        {
            var course = await _courseRepository.GetCourseByNameAsync(CName);
            if (course == null)
            {
                throw new Exception("Course not found");
            }
            return course;
        }

        public async Task<Courses> CreateCourseAsync(Courses course)
        {
            return await _courseRepository.CreateCourseAsync(course);
        }

        public async Task<Courses> UpdateCourseAsync(Courses course)
        {
            return await _courseRepository.UpdateCourseAsync(course);
        }

        public async Task<bool> DeleteCourseAsync(int id)
        {
            return await _courseRepository.DeleteCourseAsync(id);
        }

        public async Task<IEnumerable<Courses>> GetCoursesByProfessorAsync(string professorId)
        {
            return await _courseRepository.GetCoursesByProfessorAsync(professorId);
        }
    }
}
