using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sh.Api.Models;

[Table("Items")]
public class GameItem
{
    [Key]
    [Column("itemid")]
    public int ItemId { get; set; }

    [Column("itemtype")]
    public short ItemType { get; set; }

    [Column("typeID")]
    public short TypeId { get; set; }

    [Column("itemname")]
    [MaxLength(64)]
    public string ItemName { get; set; } = string.Empty;

    [Column("quality")]
    public byte Quality { get; set; }

    [Column("level")]
    public byte Level { get; set; }

    [Column("country")]
    public byte Country { get; set; }

    [Column("maxdurable")]
    public short MaxDurable { get; set; }

    [Column("attackspeed")]
    public byte AttackSpeed { get; set; }

    [Column("effect")]
    public short Effect { get; set; }
}
