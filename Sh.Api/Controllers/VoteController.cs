using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sh.Api.Models;
using Sh.Api.Services;

namespace Sh.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class VoteController(VoteService voteService) : ControllerBase
{
    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetVoteStatus()
    {
        var userUid = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var status = await voteService.GetVoteStatusAsync(userUid);
        return Ok(ApiResponse<VoteDto>.Ok(status));
    }

    [HttpPost("claim")]
    [Authorize]
    public async Task<IActionResult> ClaimVote([FromBody] VoteClaimRequest request)
    {
        var userUid = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var (success, message, newBalance) = await voteService.ClaimVoteAsync(userUid, request.VoteSite);

        if (!success) return BadRequest(ApiResponse<object>.Fail(message));
        return Ok(ApiResponse<object>.Ok(new { newBalance }, message));
    }

    // Callback from GTop100 vote site
    [HttpGet("callback/gtop")]
    [AllowAnonymous]
    public async Task<IActionResult> GtopCallback([FromQuery] int userid, [FromQuery] string secret = "", [FromQuery] string voted = "")
    {
        try
        {
            await voteService.ProcessExternalVoteAsync(userid, "GTop100", Request.Headers["CF-Connecting-IP"].ToString() ?? HttpContext.Connection.RemoteIpAddress?.ToString(), secret);
            return Ok("OK");
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    // Callback from Xtremetop100 vote site
    [HttpGet("callback/xtreme")]
    [AllowAnonymous]
    public async Task<IActionResult> XtremeCallback([FromQuery] int userid)
    {
        try
        {
            await voteService.ProcessExternalVoteAsync(userid, "Xtremetop100", null, null);
            return Ok("OK");
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}
