using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sh.Api.Data;
using Sh.Api.Models;
using Sh.Api.Services;

namespace Sh.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PublicController(AppDbContext db, GameDbContext gameDb, ServerManager serverManager) : ControllerBase
{
    [HttpGet("home")]
    public async Task<IActionResult> GetHome()
    {
        var news = await db.News
            .Where(n => n.IsPublished)
            .OrderByDescending(n => n.CreatedAt)
            .Take(5)
            .ToListAsync();

        var pvpRanking = await gameDb.Characters
            .Where(c => c.Del == 0 && c.K1 > 0)
            .OrderByDescending(c => c.K1)
            .Take(10)
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

        var serverStatus = await serverManager.GetStatusAsync();

        return Ok(ApiResponse<HomeDto>.Ok(new HomeDto
        {
            LatestNews = news,
            PvpRanking = pvpRanking,
            ServerStatus = serverStatus
        }));
    }

    [HttpGet("news")]
    public async Task<IActionResult> GetNews([FromQuery] int page = 1, [FromQuery] int pageSize = 10, [FromQuery] string? category = null)
    {
        var query = db.News.Where(n => n.IsPublished);
        if (!string.IsNullOrEmpty(category))
            query = query.Where(n => false); // category column no longer exists in the table

        var total = await query.CountAsync();
        var items = await query
            .OrderByDescending(n => n.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return Ok(ApiResponse<PagedResult<News>>.Ok(new PagedResult<News>
        {
            Items = items,
            Total = total,
            Page = page,
            PageSize = pageSize
        }));
    }

    [HttpGet("news/{id:int}")]
    public async Task<IActionResult> GetNewsItem(int id)
    {
        var news = await db.News.FirstOrDefaultAsync(n => n.Id == id && n.IsPublished);
        if (news == null) return NotFound(ApiResponse<object>.Fail("News not found."));
        return Ok(ApiResponse<News>.Ok(news));
    }

    [HttpGet("pvp-ranking")]
    public async Task<IActionResult> GetPvpRanking([FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        var total = await gameDb.Characters.Where(c => c.Del == 0 && c.K1 > 0).CountAsync();
        var items = await gameDb.Characters
            .Where(c => c.Del == 0 && c.K1 > 0)
            .OrderByDescending(c => c.K1)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
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

        return Ok(ApiResponse<PagedResult<CharacterDto>>.Ok(new PagedResult<CharacterDto>
        {
            Items = items,
            Total = total,
            Page = page,
            PageSize = pageSize
        }));
    }

    [HttpGet("server-status")]
    public async Task<IActionResult> GetServerStatus()
    {
        var status = await serverManager.GetStatusAsync();
        return Ok(ApiResponse<ServerStatusDto>.Ok(status));
    }
}
