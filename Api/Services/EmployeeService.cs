using Api.Interfaces;
using Data.Entities;
using Data.Interfaces;
using Data.Repositories;

namespace Api.Services;

public class EmployeeService(IEmployeeRepository employeeRepository, IRepository<RoleEntity> roleRepository, ILogger<ProjectService> logger) : IEmployeeService
{
    private readonly ILogger<ProjectService> _logger = logger;
    public async Task<EmployeeEntity> CreateEmployeeAsync(EmployeeEntity employee)
    {
        using (var transaction = await roleRepository.BeginTransactionAsync())
        {
            try
            {
                _logger.LogInformation("Skapar anställd: {EmployeeEmail}", employee.Email);
                
                var roleExists = await roleRepository.GetByIdAsync(employee.RoleId);
                if (roleExists == null)
                {
                    _logger.LogWarning("Ingen roll med ID: {RoleId} hittades.", employee.RoleId);
                    throw new ArgumentException($"Ingen roll med ID: {employee.RoleId} hittades.");
                }

                employee.RoleId = roleExists.Id;
            
                var createdEmployee = await employeeRepository.AddAsync(employee);
                await transaction.CommitAsync();
                _logger.LogInformation("Anställd {EmployeeEmail} har skapats.", employee.Email);
                
                return createdEmployee;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "Anställd {EmployeeEmail} kunde INTE skapas.", employee.Email);
                throw;
            }
        }
    }


    
    public async Task<IEnumerable<EmployeeEntity>> GetAllEmployeesAsync()
    {
        return await employeeRepository.GetAllEmployeesWithRolesAsync();
    }

    public async Task<EmployeeEntity?> GetEmployeeByIdAsync(int id)
    {
        return await employeeRepository.GetEmployeeWithRoleAsync(id);
    }

    public async Task UpdateEmployeeAsync(EmployeeEntity employee)
    {
        using (var transaction = await roleRepository.BeginTransactionAsync())
        {
            try
            {
                _logger.LogInformation("Uppdaterar anställd: {EmployeeEmail}", employee.Email);
                
                await employeeRepository.UpdateAsync(employee);
                _logger.LogInformation("{EmployeeEmail} har uppdaterats!", employee.Email);
                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "Anställd {EmployeeEmail} kunde INTE uppdateras.", employee.Email);
                throw;
            }
        }
    }

    public async Task DeleteEmployeeAsync(int id)
    {
        using (var transaction = await roleRepository.BeginTransactionAsync())
        {
            try
            {
                _logger.LogInformation("Anställd med ID: {EmployeeId} tas bort", id);
                await employeeRepository.DeleteAsync(id);
                await transaction.CommitAsync();
                _logger.LogInformation("Anställd med ID: {EmployeeId} har tagits bort", id);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "Anställd med ID: {EmployeeId} kunde INTE tas bort.", id);
                throw;
            }
        }
    }
    
    public async Task<IEnumerable<EmployeeEntity>> GetProjectManagersAsync()
    {
        return await employeeRepository.GetProjectManagersAsync();
    }
}