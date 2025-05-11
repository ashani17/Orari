using Orari.Models;
using System.Collections;

namespace Orari.Interfaces
{
    public interface IDepartmentRepository
    {
        Task<IEnumerable<Departments>> GetAllDepartmentsAsync();
        Task<Departments> GetDepartmentByIdAsync(int id);
        Task<Departments> CreateDepartmentAsync(Departments department);
        Task<Departments> UpdateDepartmentAsync(Departments department);
        Task<bool> DeleteDepartmentAsync(int id);
        Task<Departments> GetDepartmentByNameAsync(string name);
    }
}
