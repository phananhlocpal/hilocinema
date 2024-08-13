using EmployeeService.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeeService.Repositories
{
    public class EmployeeRepo : IEmployeeRepo
    {
        private readonly EmployeeContext _context;

        public EmployeeRepo(EmployeeContext context)
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
            if (employee == null)
            {
                throw new ArgumentNullException(nameof(employee));
            }

            await _context.Employees.AddAsync(employee);
        }

        public async Task UpdateAsync(Employee employee)
        {
            if (employee == null)
            {
                throw new ArgumentNullException(nameof(employee));
            }

            var existingEmployee = await _context.Employees.FindAsync(employee.Id);
            if (existingEmployee == null)
            {
                throw new KeyNotFoundException($"Employee with Id {employee.Id} not found.");
            }

            // Update existing employee's properties
            existingEmployee.Name = employee.Name;
            existingEmployee.Email = employee.Email;
            existingEmployee.Address = employee.Address;
            existingEmployee.Phone = employee.Phone;
            existingEmployee.Gender = employee.Gender;
            existingEmployee.Birthdate = employee.Birthdate;
            existingEmployee.Password = employee.Password;
            existingEmployee.Position = employee.Position;
            existingEmployee.SysRole = employee.SysRole;
            existingEmployee.Token = employee.Token;
            existingEmployee.CreatedDate = employee.CreatedDate;
            existingEmployee.Status = employee.Status;

            _context.Employees.Update(existingEmployee);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() >= 0;
        }
    }
}
