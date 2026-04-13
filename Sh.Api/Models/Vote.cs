using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sh.Api.Models;

[Table("Vote")]
public class Vote
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("useruid")]
    public int UserUid { get; set; }

    [Column("votesite")]
    [MaxLength(100)]
    public string VoteSite { get; set; } = string.Empty;

    [Column("votedat")]
    public DateTime VotedAt { get; set; }

    [Column("pointsawarded")]
    public int PointsAwarded { get; set; }
}
