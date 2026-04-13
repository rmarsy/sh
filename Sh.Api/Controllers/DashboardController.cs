using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sh.Api.Data;
using Sh.Api.Models;
using Sh.Api.Services;

namespace Sh.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class DashboardController(UserDbContext userDb, GameDbContext gameDb, AuthService authService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetDashboard()
    {
        var userUid = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var user = await userDb.Users.FindAsync(userUid);
        if (user == null) return Unauthorized();

        var characters = await gameDb.Characters
            .Where(c => c.UserUid == userUid && c.Del == 0)
            .Select(c => new CharacterDto
            {
                CharId = c.CharId,
                ServerId = c.ServerId,
                CharName = c.CharName,
                Family = c.Family,
                Grow = c.Grow,
                Job = c.Job,
                Sex = c.Sex,
                Level = c.Level,
                K1 = c.K1,
                Money = c.Money,
                LoginStatus = c.LoginStatus,
                Map = c.Map
            })
            .ToListAsync();

        return Ok(ApiResponse<DashboardDto>.Ok(new DashboardDto
        {
            User = authService.MapToDto(user),
            Characters = characters
        }));
    }
}
