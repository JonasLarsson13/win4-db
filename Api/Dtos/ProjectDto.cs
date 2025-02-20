using System.ComponentModel.DataAnnotations;

namespace Api.Dtos;

public class ProjectDto
{
    [Required]
    public string ProjectNumber { get; set; } = string.Empty;

    [Required]
    public string Name { get; set; } = string.Empty;

    [Required]
    public DateTime StartDate { get; set; }

    [Required]
    public DateTime EndDate { get; set; }

    [Required]
    public string Hours { get; set; } = string.Empty;

    [Required]
    public decimal Amount { get; set; }

    public string Note { get; set; } = string.Empty;

    [Required]
    public int StatusId { get; set; }

    [Required]
    public int ProjectLeaderId { get; set; }

    public List<int> ProductIds { get; set; } = [];
    public List<int> CustomerIds { get; set; } = [];
}


public class ProjectSummaryDto
{
    public string ProjectNumber { get; set; } = null!;
    public string Name { get; set; } = null!;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string Status { get; set; } = null!;
    public List<string> Customers { get; set; } = [];
    public ProjectLeaderDto ProjectLeader { get; set; } = null!;
}

public class ProjectDetailsDto
{
    public string ProjectNumber { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string Note { get; set; } = null!;
    public string Hours { get; set; } = null!;
    public decimal Amount { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    
    public string Status { get; set; } = null!;
    
    public ProjectLeaderDto ProjectLeader { get; set; } = null!;
    public List<ProductDto> Products { get; set; } = [];
    public List<CustomerDto> Customers { get; set; } = [];
}
