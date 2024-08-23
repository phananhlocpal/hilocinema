using AuthenticationService.Models;

namespace AuthenticationService.Repositories.EmployeeRepositories
{
    public interface IEmployeeRepo
    {
        Task<IEnumerable<Employee>> GetAllAsync();
        Task<Employee> GetByIdAsync(int id);
        Task CreateAsync(Employee employee);
        Task UpdateAsync(Employee employee);
        Task<bool> SaveChangesAsync();
        Task HideEmployeeAsync(int id);
        Task<bool> EmailExistsAsync(string email);
    }
}
