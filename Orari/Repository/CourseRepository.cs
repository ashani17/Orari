using Microsoft.EntityFrameworkCore;
using Orari.DataDbContext;
using Orari.Interfaces;
using Orari.Models;

namespace Orari.Repository
{
    public class CourseRepository : ICourseRepository
    {
        private readonly AppDbContext _context;

        public CourseRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Courses> CreateCourseAsync(Courses course)
        {
            await _context.Courses.AddAsync(course);
            await _context.SaveChangesAsync();
            return course;
        }

        public async Task<bool> DeleteCourseAsync(int id)
        {
            var course = await _context.Courses.FindAsync(id);
            if (course == null) return false;

            _context.Courses.Remove(course);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Courses>> GetAllCourses()
        {
            return await _context.Courses.ToListAsync();
        }

        public async Task<Courses> GetCourseByIdAsync(int id)
        {
            var course = await _context.Courses.FirstOrDefaultAsync(c => c.CId == id);
            if (course == null) throw new Exception("Course not found");
            return course;
        }

        public async Task<Courses?> GetCourseByNameAsync(string CName)
        {
            var course = await _context.Courses.FirstOrDefaultAsync(c => c.CName == CName);
            if (course == null) throw new Exception("Course not found");
            return course;
        }

        public async Task<Courses> UpdateCourseAsync(Courses course)
        {
           var existingCourse = await _context.Courses.FindAsync(course.CId);
            if (existingCourse == null) throw new Exception("Course not found");
            existingCourse.CName = course.CName;
            _context.Courses.Update(course);
            await _context.SaveChangesAsync();
            return course;
        }
    }
}
