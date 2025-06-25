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

        public async Task<IEnumerable<Courses>> GetAllCoursesAsync()
        {
            return await _context.Courses
                .Include(c => c.Enrollments)
                .Include(c => c.StudyProgramCourse)
                    .ThenInclude(spc => spc.StudyProgram)
                .ToListAsync();
        }

        public async Task<Courses?> GetCourseByIdAsync(int id)
        {
            return await _context.Courses
                .Include(c => c.Enrollments)
                .Include(c => c.StudyProgramCourse)
                    .ThenInclude(spc => spc.StudyProgram)
                .FirstOrDefaultAsync(c => c.CId == id);
        }

        public async Task<Courses?> GetCourseByNameAsync(string CName)
        {
            return await _context.Courses
                .Include(c => c.Enrollments)
                .Include(c => c.StudyProgramCourse)
                .FirstOrDefaultAsync(c => c.CName == CName);
        }

        public async Task<Courses> CreateCourseAsync(Courses course)
        {
            _context.Courses.Add(course);
            await _context.SaveChangesAsync();
            return course;
        }

        public async Task<Courses> UpdateCourseAsync(Courses course)
        {
            // Remove old StudyProgramCourse relationships
            var oldLinks = _context.StudyProgramCourses.Where(spc => spc.CId == course.CId);
            _context.StudyProgramCourses.RemoveRange(oldLinks);

            // Add new StudyProgramCourse relationships from the course object
            foreach (var spc in course.StudyProgramCourse)
            {
                _context.StudyProgramCourses.Add(spc);
            }

            _context.Courses.Update(course);
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

        public async Task<IEnumerable<Courses>> GetCoursesByProfessorAsync(string professorId)
        {
            return await _context.Courses
                .Where(c => c.Profesor == professorId)
                .Include(c => c.Enrollments)
                .Include(c => c.StudyProgramCourse)
                .ToListAsync();
        }

        public async Task AddCourseToStudyProgramAsync(StudyProgramCourse studyProgramCourse)
        {
            await _context.StudyProgramCourses.AddAsync(studyProgramCourse);
            await _context.SaveChangesAsync();
        }
    }
}
