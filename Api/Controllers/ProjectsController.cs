using Api.Dtos;
using Api.Factories;
using Api.Interfaces;
using Data.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class ProjectsController(IProjectService projectService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAllProjects()
    {
        return Ok(await projectService.GetAllProjectsAsync());
    }

    [HttpGet("{projectNumber}")]
    public async Task<IActionResult> GetProjectByNumber(string projectNumber)
    {
        var project = await projectService.GetProjectByNumberAsync(projectNumber);
    
        if (project == null) return NotFound();
    
        return Ok(project);
    }

    [HttpPost]
    public async Task<IActionResult> CreateProject([FromBody] ProjectDto projectDto)
    {
        var createdProject = await projectService.CreateProjectAsync(projectDto, projectDto.ProductIds, projectDto.CustomerIds);
        return CreatedAtAction(nameof(GetProjectByNumber), new { projectNumber = createdProject.ProjectNumber }, createdProject);
    }


    [HttpPut("{projectNumber}")]
    public async Task<IActionResult> UpdateProject(string projectNumber, [FromBody] ProjectDto projectDto)
    {
        if (projectNumber != projectDto.ProjectNumber)
        {
            return BadRequest("Projektnummer i URL och body matchar inte.");
        }

        try
        {
            var updatedProject = ProjectFactory.CreateFromUpdate(projectDto);
            
            var updatedProjectDto = await projectService.UpdateProjectAsync(projectDto, projectDto.ProductIds, projectDto.CustomerIds);
            return Ok(updatedProjectDto);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Ett internt fel uppstod: {ex.Message}");
        }
    }

    [HttpDelete("{projectNumber}")]
    public async Task<IActionResult> DeleteProject(string projectNumber)
    {
        await projectService.DeleteProjectAsync(projectNumber);
        return NoContent();
    }
}