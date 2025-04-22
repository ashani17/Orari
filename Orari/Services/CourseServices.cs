using Orari.Interfaces;
using Orari.Models;

namespace Orari.Services
{
    public class CourseServices : ICourseService
    {
        private readonly ICourseService _course;
        public CourseServices(ICourseService course)
        {
            _course = course;
        }
        public async Task<Courses> CreateCourseAsync(Courses course, string CName)
        {
            var existingCourse = GetCourseByNameAsync(CName);
            if (existingCourse != null)
            {
                throw new Exception("Course already exists");
            }

            return await _course.CreateCourseAsync(course, CName);
        }

        public Task<bool> DeleteCourseAsync(int id)
        {
            var existingCourse = GetCourseByIdAsync(id);
            if (existingCourse == null)
            {
                throw new Exception("Course not found");
            }
            return _course.DeleteCourseAsync(id);
        }

        public Task<IEnumerable<Courses>> GetAllCourses()
        {
            return _course.GetAllCourses();
        }

        public Task<Courses> GetCourseByIdAsync(int id)
        {
            return _course.GetCourseByIdAsync(id);
        }

        public Task<Courses?> GetCourseByNameAsync(string CName)
        {
            return _course.GetCourseByNameAsync(CName);
        }

        public Task<bool> UpdateCourseAsync(int id, Courses course)
        {
            var existingCourse = GetCourseByIdAsync(id);
            if (existingCourse == null)
            {
                throw new Exception("Course not found");
            }
            return _course.UpdateCourseAsync(id, course);
        }
    }
}
