using Orari.Models;

namespace Orari.Interfaces
{
    public interface IStudyProgramService
    {
        Task<IEnumerable<StudyPrograms>> GetAllStudyProgramAsync();
        Task<StudyPrograms> GetStudyProgramByIdAsync(int id);
        Task<StudyPrograms> CreateStudyProgramAsync(StudyPrograms studyProgram);
        Task<StudyPrograms> UpdateStudyProgramAsync(StudyPrograms studyProgram);
        Task<bool> DeleteStudyProgramAsync(int id);
        Task<StudyPrograms> GetStudyProgramsByNameAsync(string name);
    }
}
