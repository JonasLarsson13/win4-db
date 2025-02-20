using Api.Dtos;
using Api.Factories;
using Api.Interfaces;
using Data.Entities;
using Data.Interfaces;
using Data.Repositories;

namespace Api.Services;

public class ProjectService(
    IProjectRepository projectRepository,
    IRepository<ProductEntity> productRepository,
    IRepository<CustomerEntity> customerRepository,
    ILogger<ProjectService> logger
) : IProjectService
{
    private readonly IProjectRepository _projectRepository = projectRepository;
    private readonly IRepository<ProductEntity> _productRepository = productRepository;
    private readonly IRepository<CustomerEntity> _customerRepository = customerRepository;
    private readonly ILogger<ProjectService> _logger = logger;
    
    public async Task<ProjectSummaryDto> CreateProjectAsync(ProjectDto projectDto, List<int> productIds, List<int> customerIds)
    {
        using var transaction = await _projectRepository.BeginTransactionAsync();
        try
        {
            _logger.LogInformation("Skapar projekt: {ProjectNumber}", projectDto.ProjectNumber);
            
            var project = ProjectFactory.Create(projectDto);
            
            var existingProject = await _projectRepository.GetProjectByNumberAsync(project.ProjectNumber);
            if (existingProject != null)
            {
                _logger.LogWarning("Projekt med {ProjectNumber} finns redan", projectDto.ProjectNumber);
                throw new ArgumentException($"Projekt med nummer {project.ProjectNumber} finns redan.");
            }

            var status = await _projectRepository.GetStatusByIdAsync((int)project.StatusId);
            if (status == null)
            {
                _logger.LogWarning("Ingen status hittades med ID {StatusId}", project.StatusId);
                throw new ArgumentException($"Ingen status hittades med ID {project.StatusId}");
            }
            project.StatusType = status;

            var projectLeader = await _projectRepository.GetProjectLeaderByIdAsync(project.ProjectLeaderId);
            if (projectLeader == null)
            {
                _logger.LogWarning("Ingen projektledare hittades med ID {ProjectLeaderId}", project.ProjectLeaderId);
                throw new ArgumentException($"Ingen projektledare hittades med ID {project.ProjectLeaderId}");
            }
            project.ProjectLeader = projectLeader;
            
            var products = await _productRepository.GetAllAsync();
            project.Products = products.Where(p => productIds.Contains(p.Id)).ToList();

            var customers = await _customerRepository.GetAllAsync();
            project.Customers = customers.Where(c => customerIds.Contains(c.Id)).ToList();

            var createdProject = await _projectRepository.AddProjectAsync(project);
                
            await transaction.CommitAsync();
            _logger.LogInformation("Projekt {ProjectNumber} har skapats", projectDto.ProjectNumber);
            
            return new ProjectSummaryDto
            {
                ProjectNumber = createdProject.ProjectNumber,
                Name = createdProject.Name,
                StartDate = createdProject.StartDate,
                EndDate = createdProject.EndDate,
                Status = createdProject.StatusType.StatusName,
                Customers = createdProject.Customers.Select(c => c.CustomerName).ToList(),
                ProjectLeader = new ProjectLeaderDto
                {
                    Id = createdProject.ProjectLeader.Id,
                    FullName = $"{createdProject.ProjectLeader.FirstName} {createdProject.ProjectLeader.LastName}",
                    Email = createdProject.ProjectLeader.Email,
                    Role = createdProject.ProjectLeader.Role != null 
                        ? createdProject.ProjectLeader.Role.RoleName 
                        : "Projektledare"
                }
            };
        }
        catch (Exception e)
        {
            await transaction.RollbackAsync();
            _logger.LogError(e, "Projekt: {ProjectNumber}, kunde inte skapas! ", projectDto.ProjectNumber);
            throw;
        }
    }

    public async Task<IEnumerable<ProjectSummaryDto>> GetAllProjectsAsync()
    {
        var projects = await _projectRepository.GetAllProjectsAsync();

        return projects.Select(p => new ProjectSummaryDto
        {
            ProjectNumber = p.ProjectNumber,
            Name = p.Name,
            StartDate = p.StartDate,
            EndDate = p.EndDate,
            Status = p.StatusType.StatusName,
            Customers = p.Customers.Select(c => c.CustomerName).ToList(),
            ProjectLeader = p.ProjectLeader != null 
                ? new ProjectLeaderDto
                {
                    Id = p.ProjectLeader.Id,
                    FullName = $"{p.ProjectLeader.FirstName} {p.ProjectLeader.LastName}",
                    Email = p.ProjectLeader.Email,
                    Role = p.ProjectLeader.Role != null 
                        ? p.ProjectLeader.Role.RoleName 
                        : "Projektledare"
                }
                : null
        }).ToList();
    }

    public async Task<ProjectDetailsDto?> GetProjectByNumberAsync(string projectNumber)
    {
        try
        {
            _logger.LogInformation("Hämtar projekt: {ProjectNumber}", projectNumber);
        
            var project = await _projectRepository.GetProjectByNumberAsync(projectNumber);

            if (project == null)
            {
                _logger.LogWarning("Ingen projekt med projektnummer: {ProjectNumber} hittades", projectNumber);
                return null;
            }

            _logger.LogInformation("Projekt {ProjectNumber} hittades", projectNumber);
        
            return new ProjectDetailsDto
            {
                ProjectNumber = project.ProjectNumber,
                Name = project.Name,
                Note = project.Note,
                Hours = project.Hours,
                Amount = project.Amount,
                StartDate = project.StartDate,
                EndDate = project.EndDate,
                Status = project.StatusType.StatusName, 

                ProjectLeader = new ProjectLeaderDto
                {
                    Id = project.ProjectLeader.Id,
                    FullName = $"{project.ProjectLeader.FirstName} {project.ProjectLeader.LastName}",
                    Email = project.ProjectLeader.Email,
                    Role = project.ProjectLeader.Role != null 
                        ? project.ProjectLeader.Role.RoleName 
                        : "Projektledare"
                },

                Products = project.Products.Select(p => new ProductDto
                {
                    Id = p.Id,
                    ProductName = p.ProductName,
                    Price = p.Price
                }).ToList(),

                Customers = project.Customers.Select(c => new CustomerDto
                {
                    Id = c.Id,
                    CustomerName = c.CustomerName
                }).ToList()
            };
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Fel vid hämtning av projekt med ID {ProjectNumber}", projectNumber);
            throw;
        }
    }

    public async Task<ProjectDetailsDto> UpdateProjectAsync(ProjectDto projectDto, List<int> productIds, List<int> customerIds)
    {
        using (var transaction = await _projectRepository.BeginTransactionAsync())
        {
            try
            {
                _logger.LogInformation("Påbörjar uppdatering av projekt: {ProjectNumber}", projectDto.ProjectNumber);
                
                var updatedProject = ProjectFactory.CreateFromUpdate(projectDto);
                
                var existingProject = await _projectRepository.GetProjectByNumberAsync(updatedProject.ProjectNumber);
                if (existingProject == null)
                {
                    _logger.LogWarning("Projekt med nummer: {ProjectNumber} hittades inte.", projectDto.ProjectNumber);
                    throw new KeyNotFoundException($"Projekt med nummer {updatedProject.ProjectNumber} hittades inte.");
                }

                var status = await _projectRepository.GetStatusByIdAsync((int)updatedProject.StatusId);
                if (status == null)
                {
                    _logger.LogWarning("Ingen status med ID {StatusId} hittades", updatedProject.StatusId);
                    throw new ArgumentException($"Ingen status hittades med ID {updatedProject.StatusId}");
                }

                existingProject.StatusType = status;

                existingProject.StatusId = (int)projectDto.StatusId;

                var projectLeader = await _projectRepository.GetProjectLeaderByIdAsync(updatedProject.ProjectLeaderId);
                if (projectLeader == null)
                {
                    _logger.LogWarning("Ingen projektledare med ID {ProjectLeaderId} hittades.", updatedProject.ProjectLeaderId);
                    throw new ArgumentException(
                        $"Ingen projektledare hittades med ID {updatedProject.ProjectLeaderId}");
                }

                existingProject.ProjectLeader = projectLeader;

                existingProject.Name = updatedProject.Name;
                existingProject.StartDate = updatedProject.StartDate;
                existingProject.EndDate = updatedProject.EndDate;
                existingProject.StatusId = updatedProject.StatusId;
                existingProject.Hours = updatedProject.Hours;
                existingProject.ProjectLeaderId = updatedProject.ProjectLeaderId;
                existingProject.Amount = updatedProject.Amount;
                existingProject.Note = updatedProject.Note;

                existingProject.Products.Clear();
                var products = await _productRepository.GetAllAsync();
                existingProject.Products = products.Where(p => productIds.Contains(p.Id)).ToList();

                existingProject.Customers.Clear();
                var customers = await _customerRepository.GetAllAsync();
                existingProject.Customers = customers.Where(c => customerIds.Contains(c.Id)).ToList();

                await _projectRepository.UpdateProjectAsync(existingProject);
                
                await transaction.CommitAsync();
                _logger.LogInformation("Projekt {ProjectNumber} har uppdaterats.", projectDto.ProjectNumber);
                
                return new ProjectDetailsDto
                {
                    ProjectNumber = existingProject.ProjectNumber,
                    Name = existingProject.Name,
                    Note = existingProject.Note,
                    Hours = existingProject.Hours,
                    Amount = existingProject.Amount,
                    StartDate = existingProject.StartDate,
                    EndDate = existingProject.EndDate,
                    Status = existingProject.StatusType.StatusName,

                    ProjectLeader = new ProjectLeaderDto
                    {
                        Id = existingProject.ProjectLeader.Id,
                        FullName =
                            $"{existingProject.ProjectLeader.FirstName} {existingProject.ProjectLeader.LastName}",
                        Email = existingProject.ProjectLeader.Email,
                        Role = existingProject.ProjectLeader.Role != null
                            ? existingProject.ProjectLeader.Role.RoleName
                            : "Projektledare"
                    },

                    Products = existingProject.Products.Select(p => new ProductDto
                    {
                        Id = p.Id,
                        ProductName = p.ProductName,
                        Price = p.Price
                    }).ToList(),

                    Customers = existingProject.Customers.Select(c => new CustomerDto
                    {
                        Id = c.Id,
                        CustomerName = c.CustomerName
                    }).ToList()
                };
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "Något gick fel vid uppdatering av {ProjectNumber}.", projectDto.ProjectNumber);
                throw;
            }
        }
    }

    public async Task DeleteProjectAsync(string projectNumber)
    {
        using (var transaction = await _projectRepository.BeginTransactionAsync())
        {
            try
            {
                _logger.LogInformation("Tar bort projekt med projektnummer: {ProjectNumber}", projectNumber);
                await _projectRepository.DeleteProjectAsync(projectNumber);
                await transaction.CommitAsync();
                _logger.LogInformation("Projekt {ProjectNumber} har tagits bort.", projectNumber);
            }
            catch
            {
                await transaction.RollbackAsync();
                _logger.LogError("Det gick inte att ta bort projekt med projektnummer: {ProjectNumber}", projectNumber);
                throw;
            }
        }
    }
}
