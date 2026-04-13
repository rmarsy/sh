using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sh.Api.Models;

[Table("RedeemCodes")]
public class RedeemCode
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("code")]
    [MaxLength(50)]
    public string Code { get; set; } = string.Empty;

    [Column("description")]
    [MaxLength(255)]
    public string? Description { get; set; }

    [Column("maxuses")]
    public int MaxUses { get; set; }

    [Column("currentuses")]
    public int CurrentUses { get; set; }

    [Column("expiresat")]
    public DateTime? ExpiresAt { get; set; }

    [Column("active")]
    public bool Active { get; set; }

    [Column("createdat")]
    public DateTime CreatedAt { get; set; }
}

[Table("RedeemCodeItems")]
public class RedeemCodeItem
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("redeemcodeid")]
    public int RedeemCodeId { get; set; }

    [Column("itemtype")]
    public short ItemType { get; set; }

    [Column("typeid")]
    public short TypeId { get; set; }

    [Column("amount")]
    public int Amount { get; set; }

    [Column("points")]
    public int Points { get; set; }
}

[Table("RedeemCodeLogs")]
public class RedeemCodeLog
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("redeemcodeid")]
    public int RedeemCodeId { get; set; }

    [Column("useruid")]
    public int UserUid { get; set; }

    [Column("redeemedat")]
    public DateTime RedeemedAt { get; set; }
}
