using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sh.Api.Data;
using Sh.Api.Models;
using Sh.Api.Services;

namespace Sh.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LauncherController(ServerManager serverManager, AppDbContext db) : ControllerBase
{
    [HttpGet("status")]
    public async Task<IActionResult> GetStatus()
    {
        var status = await serverManager.GetStatusAsync();
        return Ok(ApiResponse<ServerStatusDto>.Ok(status));
    }

    [HttpGet("news")]
    public async Task<IActionResult> GetNews()
    {
        var news = await db.News
            .Where(n => n.Published)
            .OrderByDescending(n => n.CreatedAt)
            .Take(10)
            .ToListAsync();

        return Ok(ApiResponse<List<News>>.Ok(news));
    }

    [HttpGet("version")]
    public IActionResult GetVersion()
    {
        return Ok(ApiResponse<object>.Ok(new
        {
            version = "1.0.0",
            minVersion = "1.0.0",
            patchUrl = "https://yourdomain.com/patch",
            forceUpdate = false
        }));
    }
}
