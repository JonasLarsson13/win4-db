using Data.Entities;
using Microsoft.EntityFrameworkCore.Storage;

namespace Data.Interfaces;

public interface IProjectRepository
{
    Task<IEnumerable<ProjectEntity>> GetAllProjectsAsync();
    Task<ProjectEntity?> GetProjectByNumberAsync(string projectNumber);
    Task<ProjectEntity> AddProjectAsync(ProjectEntity project);
    Task UpdateProjectAsync(ProjectEntity project);
    Task DeleteProjectAsync(string projectNumber);
    Task<StatusTypeEntity?> GetStatusByIdAsync(int statusId);
    Task<EmployeeEntity?> GetProjectLeaderByIdAsync(int projectLeaderId);
    Task<IDbContextTransaction> BeginTransactionAsync();
}