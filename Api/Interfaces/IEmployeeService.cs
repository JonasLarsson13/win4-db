using Data.Entities;

namespace Api.Interfaces;

public interface IEmployeeService
{
    Task<IEnumerable<EmployeeEntity>> GetAllEmployeesAsync();
    Task<EmployeeEntity?> GetEmployeeByIdAsync(int id);
    Task<EmployeeEntity> CreateEmployeeAsync(EmployeeEntity employee);
    Task UpdateEmployeeAsync(EmployeeEntity employee);
    Task DeleteEmployeeAsync(int id);
    Task<IEnumerable<EmployeeEntity>> GetProjectManagersAsync();
}