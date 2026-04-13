using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sh.Api.Models;
using Sh.Api.Services;

namespace Sh.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class BattlePassController(BattlePassService battlePassService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetBattlePass()
    {
        var userUid = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var data = await battlePassService.GetBattlePassAsync(userUid);
        return Ok(ApiResponse<BattlePassDto>.Ok(data));
    }
}
