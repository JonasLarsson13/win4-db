using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Data.Entities;

public class StatusTypeEntity
{
    [Key]
    public int Id { get; set; }
    [Column(TypeName = "nvarchar(20)")]
    public string StatusName { get; set; } = null!;
    public ICollection<ProjectEntity> Projects { get; set; } = new List<ProjectEntity>();
}