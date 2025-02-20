using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Entities;

public class CustomerContactEntity
{
    [Key]
    public int Id { get; set; }

    [Column(TypeName = "nvarchar(50)")]
    public string Name { get; set; } = null!;

    [Column(TypeName = "varchar(150)")]
    public string Email { get; set; } = null!;

    [Column(TypeName = "varchar(20)")]
    public string PhoneNumber { get; set; } = null!;
}