using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sh.Api.Data;
using Sh.Api.Models;

namespace Sh.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class DonateController(AppDbContext db) : ControllerBase
{
    [HttpGet("history")]
    public async Task<IActionResult> GetHistory()
    {
        var userUid = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var payments = await db.Payments
            .Where(p => p.UserUid == userUid)
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync();

        return Ok(ApiResponse<List<Payment>>.Ok(payments));
    }

    [HttpPost("initiate")]
    public async Task<IActionResult> Initiate([FromBody] DonateRequest request)
    {
        var userUid = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        var payment = new Payment
        {
            UserUid = userUid,
            Provider = request.Provider,
            Amount = request.Amount,
            Currency = request.Currency,
            PointsAwarded = CalculatePoints(request.Amount),
            Status = "pending",
            CreatedAt = DateTime.UtcNow
        };

        db.Payments.Add(payment);
        await db.SaveChangesAsync();

        return Ok(ApiResponse<object>.Ok(new { paymentId = payment.Id }));
    }

    [HttpPost("webhook/paypal")]
    [AllowAnonymous]
    public async Task<IActionResult> PayPalWebhook()
    {
        return Ok();
    }

    [HttpPost("webhook/stripe")]
    [AllowAnonymous]
    public async Task<IActionResult> StripeWebhook()
    {
        return Ok();
    }

    private static int CalculatePoints(decimal amount) => (int)(amount * 100);
}
