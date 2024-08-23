using AuthenticationService.Models;
using Microsoft.EntityFrameworkCore;

namespace AuthenticationService.Repositories.EmployeeRepositories
{
    public class EmployeeRepo : IEmployeeRepo
    {
        private readonly AuthenticateContext _context;

        public EmployeeRepo(AuthenticateContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Employee>> GetAllAsync()
        {
            return await _context.Employees.ToListAsync();
        }

        public async Task<Employee> GetByIdAsync(int id)
        {
            return await _context.Employees.FindAsync(id);
        }

        public async Task CreateAsync(Employee employee)
        {
            await _context.Employees.AddAsync(employee);
        }

        public async Task UpdateAsync(Employee employee)
        {
            _context.Employees.Update(employee);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task HideEmployeeAsync(int id)
        {
            var employee = await GetByIdAsync(id);
            if (employee != null)
            {
                employee.Status = "Inactive";
                _context.Employees.Update(employee);
            }
        }

        public async Task<bool> EmailExistsAsync(string email)
        {
            return await _context.Employees.AnyAsync(e => e.Email == email);
        }
    }
}
