using Api.Dtos;
using Data.Entities;

namespace Api.Factories;

public static class ProjectFactory
{
    public static ProjectEntity Create(ProjectDto projectDto)
    {
        return new ProjectEntity
        {
            ProjectNumber = projectDto.ProjectNumber,
            Name = projectDto.Name,
            StartDate = projectDto.StartDate,
            EndDate = projectDto.EndDate,
            StatusId = (int)projectDto.StatusId,
            Hours = projectDto.Hours,
            ProjectLeaderId = projectDto.ProjectLeaderId,
            Amount = projectDto.Amount,
            Note = projectDto.Note
        };
    }

    public static ProjectEntity CreateFromUpdate(ProjectDto projectUpdateDto)
    {
        return new ProjectEntity
        {
            ProjectNumber = projectUpdateDto.ProjectNumber,
            Name = projectUpdateDto.Name,
            StartDate = projectUpdateDto.StartDate,
            EndDate = projectUpdateDto.EndDate,
            StatusId = (int)projectUpdateDto.StatusId,
            Hours = projectUpdateDto.Hours,
            ProjectLeaderId = projectUpdateDto.ProjectLeaderId,
            Amount = projectUpdateDto.Amount,
            Note = projectUpdateDto.Note
        };
    }
}

