using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sh.Api.Models;

[Table("Roulette")]
public class Roulette
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("name")]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [Column("costpersp")]
    public int CostPerSpin { get; set; }

    [Column("currency")]
    [MaxLength(20)]
    public string Currency { get; set; } = "point";

    [Column("active")]
    public bool Active { get; set; }
}

[Table("RoulettePrizes")]
public class RoulettePrize
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("rouletteid")]
    public int RouletteId { get; set; }

    [Column("name")]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [Column("imageurl")]
    [MaxLength(500)]
    public string? ImageUrl { get; set; }

    [Column("itemtype")]
    public short ItemType { get; set; }

    [Column("typeid")]
    public short TypeId { get; set; }

    [Column("amount")]
    public int Amount { get; set; }

    [Column("points")]
    public int Points { get; set; }

    [Column("weight")]
    public int Weight { get; set; }
}

[Table("RouletteHistory")]
public class RouletteHistory
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("rouletteid")]
    public int RouletteId { get; set; }

    [Column("useruid")]
    public int UserUid { get; set; }

    [Column("prizeid")]
    public int PrizeId { get; set; }

    [Column("characterid")]
    public int CharacterId { get; set; }

    [Column("spunat")]
    public DateTime SpunAt { get; set; }
}
