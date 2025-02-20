using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Entities;

public class RoleEntity
{
    [Key]
    public int Id { get; set; }
    
    [Column(TypeName = "nvarchar(20)")]
    public string RoleName { get; set; } = null!;
}