﻿using Orari.Models;

namespace Orari.Interfaces
{
    public interface IExamService
    {
        Task<IEnumerable<Exams>> GetAllExams();
        Task<Exams> GetExamByIdAsync(int id);
        Task<Exams> CreateExamAsync(Exams exam);
        Task<Exams> UpdateExamAsync(Exams exam);
        Task<bool> DeleteExamAsync(int id);
    }
}
