using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sh.Api.Models;
using Sh.Api.Services;

namespace Sh.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class VoteController(VoteService voteService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetVoteStatus()
    {
        var userUid = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var status = await voteService.GetVoteStatusAsync(userUid);
        return Ok(ApiResponse<VoteDto>.Ok(status));
    }

    [HttpPost("claim")]
    public async Task<IActionResult> ClaimVote([FromBody] VoteClaimRequest request)
    {
        var userUid = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var (success, message, newBalance) = await voteService.ClaimVoteAsync(userUid, request.VoteSite);

        if (!success) return BadRequest(ApiResponse<object>.Fail(message));
        return Ok(ApiResponse<object>.Ok(new { newBalance }, message));
    }
}
