using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sh.Api.Models;

[Table("Shop")]
public class Shop
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("name")]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [Column("description")]
    [MaxLength(500)]
    public string? Description { get; set; }

    [Column("active")]
    public bool Active { get; set; }
}

[Table("ShopCategory")]
public class ShopCategory
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("shopid")]
    public int ShopId { get; set; }

    [Column("name")]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [Column("sortorder")]
    public int SortOrder { get; set; }

    [Column("active")]
    public bool Active { get; set; }
}

[Table("ShopItem")]
public class ShopItem
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("categoryid")]
    public int CategoryId { get; set; }

    [Column("name")]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [Column("description")]
    [MaxLength(1000)]
    public string? Description { get; set; }

    [Column("imageurl")]
    [MaxLength(500)]
    public string? ImageUrl { get; set; }

    [Column("price")]
    public int Price { get; set; }

    [Column("currency")]
    [MaxLength(20)]
    public string Currency { get; set; } = "point";

    [Column("itemtype")]
    public short ItemType { get; set; }

    [Column("typeid")]
    public short TypeId { get; set; }

    [Column("amount")]
    public int Amount { get; set; }

    [Column("active")]
    public bool Active { get; set; }

    [Column("featured")]
    public bool Featured { get; set; }

    [Column("sortorder")]
    public int SortOrder { get; set; }
}

[Table("ShopPurchase")]
public class ShopPurchase
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("useruid")]
    public int UserUid { get; set; }

    [Column("itemid")]
    public int ItemId { get; set; }

    [Column("characterid")]
    public int CharacterId { get; set; }

    [Column("amount")]
    public int Amount { get; set; }

    [Column("pointspent")]
    public int PointSpent { get; set; }

    [Column("purchasedat")]
    public DateTime PurchasedAt { get; set; }
}
