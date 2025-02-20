using Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
using Api.Dtos;

namespace Api.Interfaces;

public interface IProjectService
{
    Task<IEnumerable<ProjectSummaryDto>> GetAllProjectsAsync();
    Task<ProjectDetailsDto?> GetProjectByNumberAsync(string projectNumber);
    Task<ProjectSummaryDto> CreateProjectAsync(ProjectDto projectDto, List<int> productIds, List<int> customerIds);
    Task<ProjectDetailsDto> UpdateProjectAsync(ProjectDto projectDto, List<int> productIds, List<int> customerIds);
    Task DeleteProjectAsync(string projectNumber);
}
