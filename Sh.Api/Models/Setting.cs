using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sh.Api.Models;

[Table("Settings")]
public class Setting
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("key")]
    [MaxLength(100)]
    public string Key { get; set; } = string.Empty;

    [Column("value")]
    public string? Value { get; set; }

    [Column("description")]
    [MaxLength(500)]
    public string? Description { get; set; }
}
