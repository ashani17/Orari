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

        public Task<Courses> CreateCourseAsync(Courses course, string CName)
        {
            var existingCourse = _courseRepository.GetCourseByNameAsync(CName);
            if (existingCourse != null)
            {
                throw new Exception("Course already exists");
            }
            var existingProfesor = _profesorRepository.GetProfesorByIdAsync(course.PId);
            if (existingProfesor == null)
            {
                throw new Exception("Profesor not found");
            }
            return _courseRepository.CreateCourseAsync(course);
        }

        public Task<bool> DeleteCourseAsync(int id)
        {
           var existingCourse = _courseRepository.GetCourseByIdAsync(id);
            if (existingCourse == null)
            {
                throw new Exception("Course not found");
            }
            return _courseRepository.DeleteCourseAsync(id);

        }

        public Task<IEnumerable<Courses>> GetAllCourses()
        {
            return _courseRepository.GetAllCourses();
        }

        public Task<Courses> GetCourseByIdAsync(int id)
        {
            var existingCourse = _courseRepository.GetCourseByIdAsync(id);
            if (existingCourse == null)
            {
                throw new Exception("Course not found");
            }
            return _courseRepository.GetCourseByIdAsync(id);
        }

        public Task<Courses?> GetCourseByNameAsync(string CName)
        {
            var existingCourse = _courseRepository.GetCourseByNameAsync(CName);
            if (existingCourse == null)
            {
                throw new Exception("Course not found");
            }
            return _courseRepository.GetCourseByNameAsync(CName);
        }

        public Task<Courses> UpdateCourseAsync(int id, Courses course)
        {
            return _courseRepository.UpdateCourseAsync(course);
        }
    }
}
