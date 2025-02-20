using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Data.Entities;

public class EmployeeEntity
{
    [Key]
    public int Id { get; set; }

    [Column(TypeName = "nvarchar(50)")]
    public string FirstName { get; set; } = null!;

    [Column(TypeName = "nvarchar(50)")]
    public string LastName { get; set; } = null!;

    [Column(TypeName = "varchar(150)")]
    public string Email { get; set; } = null!;

    public int RoleId { get; set; }
    public RoleEntity? Role { get; set; }
    public ICollection<ProjectEntity> ProjectsAsLeader { get; set; } = new List<ProjectEntity>();
}