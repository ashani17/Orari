using Orari.Interfaces;
using Orari.Models;

namespace Orari.Services
{
    public class StudyProgramService : IStudyProgramService
    {
        private readonly IStudyProgramRepository _studyProgramRepository;
        public StudyProgramService(IStudyProgramRepository studyProgramRepository)
        {
            _studyProgramRepository = studyProgramRepository;
        }

        public async Task<StudyPrograms> CreateStudyProgramAsync(StudyPrograms studyProgram)
        {
            var existingprogram = await _studyProgramRepository.GetStudyProgramsByNameAsync(studyProgram.SPName);
            if (existingprogram != null)
            {
                throw new Exception("Study program already exists");
            }
            return await _studyProgramRepository.CreateStudyProgramAsync(studyProgram);
        }

        public async Task<bool> DeleteStudyProgramAsync(int id)
        {
            var existingprogram = await _studyProgramRepository.GetStudyProgramByIdAsync(id);
            if (existingprogram == null)
            {
                throw new Exception("Study program not found");
            }
            return await _studyProgramRepository.DeleteStudyProgramAsync(id);
        }

        public Task<IEnumerable<StudyPrograms>> GetAllStudyProgramAsync()
        {
            return _studyProgramRepository.GetAllStudyProgramAsync();
        }

        public async Task<StudyPrograms> GetStudyProgramByIdAsync(int id)
        {
           var existingprogram = await _studyProgramRepository.GetStudyProgramByIdAsync(id);
            if (existingprogram == null)
            {
                throw new Exception("Study program not found");
            }
            return await _studyProgramRepository.GetStudyProgramByIdAsync(id);
        }

        public async Task<StudyPrograms> GetStudyProgramsByNameAsync(string name)
        {
            return await _studyProgramRepository.GetStudyProgramsByNameAsync(name);
        }

        public async Task<StudyPrograms> UpdateStudyProgramAsync(StudyPrograms studyProgram)
        {
            var existingprogram = await _studyProgramRepository.GetStudyProgramByIdAsync(studyProgram.SPId);
            if (existingprogram == null)
            {
                throw new Exception("Study program not found");
            }
            return await _studyProgramRepository.UpdateStudyProgramAsync(studyProgram);
        }
    }
}
