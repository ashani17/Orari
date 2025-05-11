using Microsoft.EntityFrameworkCore;
using Orari.DataDbContext;
using Orari.DTO.DepratmentDTO;
using Orari.Interfaces;
using Orari.Models;
using System.Collections;

namespace Orari.Repository
{
    public class DepartmentRepository : IDepartmentRepository
    {
        private readonly AppDbContext _context;
        public DepartmentRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Departments> CreateDepartmentAsync(Departments department)
        {
            await _context.Departments.AddAsync(department);
            await _context.SaveChangesAsync();
            return department;
        }

        public async Task<bool> DeleteDepartmentAsync(int id)
        {
            var department = await _context.Departments.FindAsync(id);
            if (department == null) return false;
            _context.Departments.Remove(department);
            await _context.SaveChangesAsync();
            return true;
        }

        public Task<IEnumerable<Departments>> GetAllDepartmentsAsync()
        {
            var departments = _context.Departments.ToListAsync();
            if (!departments.Result.Any())
            {
                return Task.FromResult<IEnumerable<Departments>>(new List<Departments>());
            }
            return Task.FromResult<IEnumerable<Departments>>(departments.Result);
        }

        public async Task<Departments> GetDepartmentByIdAsync(int id)
        {
            var department = await _context.Departments.FirstOrDefaultAsync(d => d.DId == id);
            if (department == null) throw new Exception("Department not found");
            return department;
        }

        public async Task<Departments> GetDepartmentByNameAsync(string name)
        {
            var department = await _context.Departments.FirstOrDefaultAsync(d => d.DName == name);
            if (department == null) throw new Exception("Department not found");
            return department; // Removed incorrect usage of 'Result'
        }

        public async Task<Departments> UpdateDepartmentAsync(Departments department)
        {
            var existingDepartment = await _context.Departments.FirstOrDefaultAsync(d => d.DName == department.DName);
            if (existingDepartment == null)
                throw new Exception("Department not found");

            existingDepartment.DName = department.DName;
            await _context.SaveChangesAsync();
            return existingDepartment;
        }
    }
}
