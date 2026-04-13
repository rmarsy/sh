using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sh.Api.Models;

[Table("Chars")]
public class Character
{
    [Key]
    [Column("charid")]
    public int CharId { get; set; }

    [Column("serverid")]
    public short ServerId { get; set; }

    [Column("userid")]
    [MaxLength(20)]
    public string UserId { get; set; } = string.Empty;

    [Column("useruid")]
    public int UserUid { get; set; }

    [Column("charname")]
    [MaxLength(30)]
    public string CharName { get; set; } = string.Empty;

    [Column("new")]
    public short New { get; set; }

    [Column("del")]
    public short Del { get; set; }

    [Column("slot")]
    public short Slot { get; set; }

    [Column("family")]
    public short Family { get; set; }

    [Column("grow")]
    public short Grow { get; set; }

    [Column("hair")]
    public short Hair { get; set; }

    [Column("face")]
    public short Face { get; set; }

    [Column("size")]
    public short Size { get; set; }

    [Column("job")]
    public short Job { get; set; }

    [Column("sex")]
    public short Sex { get; set; }

    [Column("level")]
    public short Level { get; set; }

    [Column("k1")]
    public int K1 { get; set; }

    [Column("money")]
    public long Money { get; set; }

    [Column("loginstatus")]
    public bool LoginStatus { get; set; }

    [Column("map")]
    public short Map { get; set; }

    [Column("leavedate")]
    public DateTime? LeaveDate { get; set; }
}
