using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sh.Api.Models;

[Table("News")]
public class News
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("title")]
    [MaxLength(255)]
    public string Title { get; set; } = string.Empty;

    [Column("content")]
    public string Content { get; set; } = string.Empty;

    [Column("category")]
    [MaxLength(50)]
    public string Category { get; set; } = string.Empty;

    [Column("imageurl")]
    [MaxLength(500)]
    public string? ImageUrl { get; set; }

    [Column("slug")]
    [MaxLength(255)]
    public string? Slug { get; set; }

    [Column("published")]
    public bool Published { get; set; }

    [Column("createdat")]
    public DateTime CreatedAt { get; set; }

    [Column("updatedat")]
    public DateTime? UpdatedAt { get; set; }
}
