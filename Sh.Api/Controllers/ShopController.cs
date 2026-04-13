using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sh.Api.Models;
using Sh.Api.Services;

namespace Sh.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ShopController(ShopService shopService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetShop()
    {
        var shop = await shopService.GetShopAsync();
        return Ok(ApiResponse<ShopDto>.Ok(shop));
    }

    [HttpPost("purchase")]
    [Authorize]
    public async Task<IActionResult> Purchase([FromBody] PurchaseRequest request)
    {
        var userUid = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var (success, message, newBalance) = await shopService.PurchaseAsync(userUid, request.ItemId, request.CharacterId, request.Amount);

        if (!success) return BadRequest(ApiResponse<object>.Fail(message));
        return Ok(ApiResponse<object>.Ok(new { newBalance }, message));
    }
}
