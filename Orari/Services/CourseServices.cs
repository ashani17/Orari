using Orari.Interfaces;
using Orari.Models;

namespace Orari.Services
{
    public class CourseServices : ICourseService
    {
        private readonly ICourseRepository _courseRepository;
        private readonly IProfesorRepository _profesorRepository;
        private readonly IStudyProgramRepository _studyProgramRepository;

        public CourseServices(
            ICourseRepository courseRepository, 
            IProfesorRepository profesorRepository,
            IStudyProgramRepository studyProgramRepository)
        {
            _profesorRepository = profesorRepository;
            _courseRepository = courseRepository;
            _studyProgramRepository = studyProgramRepository;
        }

        public async Task<Courses> CreateCourseAsync(Courses course, int studyProgramId)
        {
            var existingCourse = await _courseRepository.GetCourseByNameAsync(course.CName);
            if (existingCourse != null)
            {
                throw new Exception("Course already exists");
            }
            var existingProfesor = await _profesorRepository.GetProfesorByEmailAsync(course.Profesor);
            if (existingProfesor == null)
            {
                throw new Exception("Profesor not found");
            }
            var studyProgram = await _studyProgramRepository.GetStudyProgramByIdAsync(studyProgramId);
            if (studyProgram == null)
            {
                throw new Exception("Study program not found");
            }
            var createdCourse = await _courseRepository.CreateCourseAsync(course);
            var studyProgramCourse = new StudyProgramCourse
            {
                CId = createdCourse.CId,
                SPId = studyProgramId
            };
            await _courseRepository.AddCourseToStudyProgramAsync(studyProgramCourse);
            return createdCourse;
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

        public async Task<IEnumerable<Courses>> GetCoursesByStudyProgramAsync(int studyProgramId)
        {
            var studyProgram = await _studyProgramRepository.GetStudyProgramByIdAsync(studyProgramId);
            if (studyProgram == null)
            {
                throw new Exception("Study program not found");
            }
            
            return await _courseRepository.GetCoursesByStudyProgramAsync(studyProgramId);
        }
    }
}
