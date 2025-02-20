using Data.Entities;
using Data.Repositories;

namespace Data.Interfaces;

public interface IEmployeeRepository : IRepository<EmployeeEntity>
{
    Task<IEnumerable<EmployeeEntity>> GetAllEmployeesWithRolesAsync();
    Task<EmployeeEntity?> GetEmployeeWithRoleAsync(int id);
    Task<IEnumerable<EmployeeEntity>> GetProjectManagersAsync();
}