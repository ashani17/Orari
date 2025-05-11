using Microsoft.EntityFrameworkCore;
using Orari.DataDbContext;
using Orari.Interfaces;
using Orari.Models;

namespace Orari.Repository
{
    public class StudyProgramRepository : IStudyProgramRepository
    {
        private readonly AppDbContext _context;
        public StudyProgramRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<StudyPrograms>> GetAllStudyProgramAsync()
        {
            return await _context.StudyPrograms.ToListAsync();
        }
        public async Task<StudyPrograms> GetStudyProgramByIdAsync(int id)
        {
            return await _context.StudyPrograms.FindAsync(id);
        }
        public async Task<StudyPrograms> CreateStudyProgramAsync(StudyPrograms studyProgram)
        {
            await _context.StudyPrograms.AddAsync(studyProgram);
            await _context.SaveChangesAsync();
            return studyProgram;
        }
        public async Task<StudyPrograms> UpdateStudyProgramAsync(StudyPrograms studyProgram)
        {
            _context.StudyPrograms.Update(studyProgram);
            await _context.SaveChangesAsync();
            return studyProgram;
        }
        public async Task<bool> DeleteStudyProgramAsync(int id)
        {
            var studyProgram = await GetStudyProgramByIdAsync(id);
            if (studyProgram == null) return false;
            _context.StudyPrograms.Remove(studyProgram);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<StudyPrograms> GetStudyProgramsByNameAsync(string name)
        {
            return await _context.StudyPrograms.FirstOrDefaultAsync(sp => sp.SPName == name);
        }
    }
}
