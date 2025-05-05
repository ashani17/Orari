using Orari.Interfaces;
using Orari.Models;

namespace Orari.Services
{
    public class CourseServices : ICourseService
    {
        private readonly ICourseRepository _courseRepository;
        private readonly IProfesorRepository _profesorRepository;
        public CourseServices(ICourseRepository courseRepository, IProfesorRepository profesorRepository)
        {
            _profesorRepository = profesorRepository;
            _courseRepository = courseRepository;
        }

        public async Task<Courses> CreateCourseAsync(Courses course, string CName)
        {
            var existingCourse = await _courseRepository.GetCourseByNameAsync(CName);
            if (existingCourse != null)
            {
                throw new Exception("Course already exists");
            }
            var existingProfesor = await _profesorRepository.GetProfesorByEmailAsync(course.Profesor);
            if (existingProfesor == null)
            {
                throw new Exception("Profesor not found");
            }
            return await _courseRepository.CreateCourseAsync(course);
        }

        public async Task<bool> DeleteCourseAsync(int id)
        {
           var existingCourse = _courseRepository.GetCourseByIdAsync(id);
            if (existingCourse == null)
            {
                throw new Exception("Course not found");
            }
            return await _courseRepository.DeleteCourseAsync(id);

        }

        public async Task<IEnumerable<Courses>> GetAllCourses()
        {
            return await _courseRepository.GetAllCourses();
        }

        public async Task<Courses> GetCourseByIdAsync(int id)
        {
            var existingCourse = await _courseRepository.GetCourseByIdAsync(id);
            if (existingCourse == null)
            {
                throw new Exception("Course not found");
            }
            return await _courseRepository.GetCourseByIdAsync(id);
        }

        public async Task<Courses?> GetCourseByNameAsync(string CName)
        {
            var existingCourse = await _courseRepository.GetCourseByNameAsync(CName);
            if (existingCourse == null)
            {
                throw new Exception("Course not found");
            }
            return await _courseRepository.GetCourseByNameAsync(CName);
        }

        public async Task<Courses> UpdateCourseAsync(int id, Courses course)
        {
            return await _courseRepository.UpdateCourseAsync(course);
        }
    }
}
