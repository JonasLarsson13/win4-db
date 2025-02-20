using Data.Contexts;
using Data.Entities;
using Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Data.Repositories;

public class ProjectRepository(DataContext context) : IProjectRepository
{
    private readonly DataContext _context = context;
    
    public async Task<ProjectEntity> AddProjectAsync(ProjectEntity project)
    {
        // Koppla endast produkter och kunder om de finns
        if (project.Products != null && project.Products.Any())
        {
            _context.Products.AttachRange(project.Products);
        }

        if (project.Customers != null && project.Customers.Any())
        {
            _context.Customers.AttachRange(project.Customers);
        }

        _context.Projects.Add(project);
        await _context.SaveChangesAsync();
        return project;
    }
    
    public async Task<IEnumerable<ProjectEntity>> GetAllProjectsAsync()
    {
        return await _context.Projects
            .Include(p => p.ProjectLeader)
            .Include(p => p.Products)
            .Include(p => p.Customers)
            .Include(p => p.StatusType)
            .ToListAsync();
    }

    public async Task<ProjectEntity?> GetProjectByNumberAsync(string projectNumber)
    {
        return await _context.Projects
            .Include(p => p.ProjectLeader)
            .Include(p => p.Products)
            .Include(p => p.Customers)
            .Include(p => p.StatusType)
            .FirstOrDefaultAsync(p => p.ProjectNumber == projectNumber);
    }

    public async Task UpdateProjectAsync(ProjectEntity project)
    {
        await _context.Entry(project).Collection(x => x.Products).LoadAsync();
        await _context.Entry(project).Collection(x => x.Customers).LoadAsync();
    
        _context.Projects.Update(project);
        await _context.SaveChangesAsync();
    }


    public async Task DeleteProjectAsync(string projectNumber)
    {
        var project = await GetProjectByNumberAsync(projectNumber);
        if (project != null)
        {
            _context.Projects.Remove(project);
            await _context.SaveChangesAsync();
        }
    }
    
    public async Task<StatusTypeEntity?> GetStatusByIdAsync(int statusId)
    {
        return await _context.StatusTypes.FirstOrDefaultAsync(s => s.Id == statusId);
    }

    public async Task<EmployeeEntity?> GetProjectLeaderByIdAsync(int projectLeaderId)
    {
        return await _context.Employees.FirstOrDefaultAsync(e => e.Id == projectLeaderId);
    }
    
    public async Task<IDbContextTransaction> BeginTransactionAsync()
    {
        return await _context.Database.BeginTransactionAsync();
    }

}