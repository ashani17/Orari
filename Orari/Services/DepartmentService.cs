using Orari.Interfaces;
using Orari.Models;

namespace Orari.Services
{
    public class DepartmentService : IDepartmentService
    {
        private readonly IDepartmentRepository _departmentRepository;
        public DepartmentService(IDepartmentRepository departmentRepository)
        {
            _departmentRepository = departmentRepository;
        }
        public async Task<Departments> CreateDepartmentAsync(Departments department)
        {
            var existingDepartment = _departmentRepository.GetDepartmentByNameAsync(department.DName);
            if (existingDepartment != null)
            {
                throw new Exception("Department already exists");
            }
            return await _departmentRepository.CreateDepartmentAsync(department);
        }
        public async Task<bool> DeleteDepartmentAsync(int id)
        {
            var existingDepartment = await _departmentRepository.GetDepartmentByIdAsync(id);
            if (existingDepartment == null)
            {
                throw new Exception("Department not found");
            }
            return await _departmentRepository.DeleteDepartmentAsync(id);
        }
        public Task<IEnumerable<Departments>> GetAllDepartments()
        {
            return _departmentRepository.GetAllDepartmentsAsync();
        }
        public async Task<Departments> GetDepartmentByIdAsync(int id)
        {
            var existingDepartment = await _departmentRepository.GetDepartmentByIdAsync(id);
            if (existingDepartment == null)
            {
                throw new Exception("Department not found");
            }
            return await _departmentRepository.GetDepartmentByIdAsync(id);
        }

        public async Task<Departments> GetDepartmentByNameAsync(string name)
        {
            var existingDepartment = await _departmentRepository.GetDepartmentByNameAsync(name);
            if (existingDepartment == null)
            {
                throw new Exception("Department not found");
            }
            return await _departmentRepository.GetDepartmentByNameAsync(name);
        }

        public async Task<Departments> UpdateDepartmentAsync(Departments department)
        {
            return await _departmentRepository.UpdateDepartmentAsync(department);
        }
    }
}
