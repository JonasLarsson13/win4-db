using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Entities;

public class ProductEntity
{
    [Key]
    public int Id { get; set; }
    [Column(TypeName = "nvarchar(100)")]
    public string ProductName { get; set; } = null!;
    [Column(TypeName = "decimal(18, 2)")]
    public decimal Price { get; set; }
    public ICollection<ProjectEntity> Projects { get; set; } = new List<ProjectEntity>();
}