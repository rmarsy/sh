using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sh.Api.Models;

[Table("Users_master")]
public class User
{
    [Key]
    [Column("useruid")]
    public int UserUid { get; set; }

    [Column("userid")]
    [MaxLength(18)]
    public string UserId { get; set; } = string.Empty;

    [Column("pw")]
    [MaxLength(255)]
    public string Pw { get; set; } = string.Empty;

    [Column("joindate")]
    public DateTime JoinDate { get; set; }

    [Column("admin")]
    public bool Admin { get; set; }

    [Column("adminlevel")]
    public int AdminLevel { get; set; }

    [Column("status")]
    public short Status { get; set; }

    [Column("leave")]
    public int Leave { get; set; }

    [Column("leavedate")]
    public DateTime? LeaveDate { get; set; }

    [Column("usertype")]
    [MaxLength(1)]
    public string UserType { get; set; } = "N";

    [Column("vp")]
    public int Vp { get; set; }

    [Column("userip")]
    [MaxLength(15)]
    public string? UserIp { get; set; }

    [Column("point")]
    public int Point { get; set; }

    [Column("securityquestion")]
    [MaxLength(255)]
    public string? SecurityQuestion { get; set; }

    [Column("securityanswer")]
    [MaxLength(255)]
    public string? SecurityAnswer { get; set; }

    [Column("resettoken")]
    [MaxLength(255)]
    public string? ResetToken { get; set; }

    [Column("resettokenexpiresat")]
    public DateTime? ResetTokenExpiresAt { get; set; }
}
