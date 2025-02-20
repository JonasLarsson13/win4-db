using Data.Contexts;
using Data.Entities;
using Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories;

public class EmployeeRepository(DataContext context) : Repository<EmployeeEntity>(context), IEmployeeRepository
{
    private readonly DataContext _context = context;

    public async Task<IEnumerable<EmployeeEntity>> GetAllEmployeesWithRolesAsync()
    {
        return await _context.Employees
            .Include(e => e.Role)
            .ToListAsync();
    }

    public async Task<EmployeeEntity?> GetEmployeeWithRoleAsync(int id)
    {
        return await _context.Employees
            .Include(e => e.Role)
            .FirstOrDefaultAsync(e => e.Id == id);
    }
    
    public async Task<IEnumerable<EmployeeEntity>> GetProjectManagersAsync()
    {
        return await _context.Employees
            .Include(e => e.Role)
            .Where(e => e.Role != null && e.Role.RoleName == "Projektledare")
            .ToListAsync();
    }
}