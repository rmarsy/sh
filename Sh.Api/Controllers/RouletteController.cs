using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sh.Api.Models;
using Sh.Api.Services;

namespace Sh.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class RouletteController(RouletteService rouletteService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetRoulette()
    {
        var roulette = await rouletteService.GetActiveRouletteAsync();
        if (roulette == null) return NotFound(ApiResponse<object>.Fail("No active roulette."));
        return Ok(ApiResponse<RouletteDto>.Ok(roulette));
    }

    [HttpPost("spin")]
    public async Task<IActionResult> Spin([FromBody] RouletteSpinRequest request)
    {
        var userUid = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var (success, message, result) = await rouletteService.SpinAsync(userUid, request.RouletteId, request.CharacterId);

        if (!success) return BadRequest(ApiResponse<object>.Fail(message));
        return Ok(ApiResponse<SpinResultDto>.Ok(result!, message));
    }
}
