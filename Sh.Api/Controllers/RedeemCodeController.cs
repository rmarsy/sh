using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sh.Api.Models;
using Sh.Api.Services;

namespace Sh.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class RedeemCodeController(RedeemCodeService redeemCodeService) : ControllerBase
{
    [HttpPost("redeem")]
    public async Task<IActionResult> Redeem([FromBody] RedeemRequest request)
    {
        var userUid = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var (success, message) = await redeemCodeService.RedeemAsync(userUid, request.Code, request.CharacterId);

        if (!success) return BadRequest(ApiResponse<object>.Fail(message));
        return Ok(ApiResponse<object>.Ok(null, message));
    }
}
