using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sh.Api.Models;

[Table("Boss_Death_Log")]
public class BossDeathLog
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("bossname")]
    [MaxLength(100)]
    public string BossName { get; set; } = string.Empty;

    [Column("killername")]
    [MaxLength(100)]
    public string KillerName { get; set; } = string.Empty;

    [Column("killtime")]
    public DateTime KillTime { get; set; }

    [Column("serverid")]
    public short ServerId { get; set; }
}
