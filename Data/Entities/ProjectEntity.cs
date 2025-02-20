using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Entities;

public class ProjectEntity
{
    [Key]
    [Column(TypeName = "nvarchar(10)")]
    public string ProjectNumber { get; set; } = null!;

    [Column(TypeName = "nvarchar(100)")]
    public string Name { get; set; } = null!;

    [Column(TypeName = "nvarchar(500)")]
    public string Note { get; set; } = null!;

    [Column(TypeName = "nvarchar(10)")]
    public string Hours { get; set; } = null!;

    [Column(TypeName = "decimal(18,2)")]
    public decimal Amount { get; set; }

    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }

    public int StatusId { get; set; }
    public StatusTypeEntity StatusType { get; set; } = null!;

    public int ProjectLeaderId { get; set; }
    public EmployeeEntity ProjectLeader { get; set; } = null!;

    public ICollection<ProductEntity> Products { get; set; } = new List<ProductEntity>();
    public ICollection<CustomerEntity> Customers { get; set; } = new List<CustomerEntity>();
}


