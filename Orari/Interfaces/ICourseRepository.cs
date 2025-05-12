using Orari.Models;

namespace Orari.Interfaces
{
    public interface ICourseRepository
    {
        Task<IEnumerable<Courses>> GetAllCourses();
        Task<Courses> GetCourseByIdAsync(int id);
        Task<Courses> CreateCourseAsync(Courses course);
        Task<Courses> UpdateCourseAsync(Courses course);
        Task<bool> DeleteCourseAsync(int id);
        Task<Courses?> GetCourseByNameAsync(string CName);
        Task AddCourseToStudyProgramAsync(StudyProgramCourse studyProgramCourse);
        Task<IEnumerable<Courses>> GetCoursesByStudyProgramAsync(int studyProgramId);
    }
}
