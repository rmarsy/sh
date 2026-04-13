using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sh.Api.Models;

[Table("Payments")]
public class Payment
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("useruid")]
    public int UserUid { get; set; }

    [Column("provider")]
    [MaxLength(50)]
    public string Provider { get; set; } = string.Empty;

    [Column("transactionid")]
    [MaxLength(255)]
    public string? TransactionId { get; set; }

    [Column("amount")]
    public decimal Amount { get; set; }

    [Column("currency")]
    [MaxLength(10)]
    public string Currency { get; set; } = "USD";

    [Column("pointsawarded")]
    public int PointsAwarded { get; set; }

    [Column("status")]
    [MaxLength(50)]
    public string Status { get; set; } = "pending";

    [Column("createdat")]
    public DateTime CreatedAt { get; set; }

    [Column("completedat")]
    public DateTime? CompletedAt { get; set; }
}
