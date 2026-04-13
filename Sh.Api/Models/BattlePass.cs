using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sh.Api.Models;

[Table("BattlePassLevels")]
public class BattlePassLevel
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("season")]
    public int Season { get; set; }

    [Column("level")]
    public int Level { get; set; }

    [Column("pointsrequired")]
    public int PointsRequired { get; set; }

    [Column("rewardname")]
    [MaxLength(100)]
    public string RewardName { get; set; } = string.Empty;

    [Column("rewardimageurl")]
    [MaxLength(500)]
    public string? RewardImageUrl { get; set; }

    [Column("itemtype")]
    public short ItemType { get; set; }

    [Column("typeid")]
    public short TypeId { get; set; }

    [Column("amount")]
    public int Amount { get; set; }
}

[Table("BattlePassTasks")]
public class BattlePassTask
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("season")]
    public int Season { get; set; }

    [Column("name")]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [Column("description")]
    [MaxLength(500)]
    public string? Description { get; set; }

    [Column("pointsreward")]
    public int PointsReward { get; set; }

    [Column("active")]
    public bool Active { get; set; }
}

[Table("BattlePassUserPoints")]
public class BattlePassUserPoints
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("useruid")]
    public int UserUid { get; set; }

    [Column("season")]
    public int Season { get; set; }

    [Column("points")]
    public int Points { get; set; }

    [Column("currentlevel")]
    public int CurrentLevel { get; set; }
}
