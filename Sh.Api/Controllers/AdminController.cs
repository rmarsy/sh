using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sh.Api.Data;
using Sh.Api.Models;

namespace Sh.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class AdminController(UserDbContext userDb, AppDbContext db, GameLogDbContext gameLogDb) : ControllerBase
{
    private bool IsAdmin => User.HasClaim("admin", "true");

    [HttpGet("users")]
    public async Task<IActionResult> GetUsers([FromQuery] int page = 1, [FromQuery] int pageSize = 20, [FromQuery] string? search = null)
    {
        if (!IsAdmin) return Forbid();

        var query = userDb.Users.AsQueryable();
        if (!string.IsNullOrEmpty(search))
            query = query.Where(u => u.UserId.Contains(search));

        var total = await query.CountAsync();
        var users = await query
            .OrderByDescending(u => u.JoinDate)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(u => new AdminUserDto
            {
                Id = u.UserUid,
                Username = u.UserId,
                Admin = u.Admin,
                AdminLevel = u.AdminLevel,
                Point = u.Point,
                Vp = u.Vp,
                Status = u.Status,
                JoinDate = u.JoinDate,
                UserIp = u.UserIp,
                Leave = u.Leave,
                LeaveDate = u.LeaveDate
            })
            .ToListAsync();

        return Ok(ApiResponse<PagedResult<AdminUserDto>>.Ok(new PagedResult<AdminUserDto>
        {
            Items = users,
            Total = total,
            Page = page,
            PageSize = pageSize
        }));
    }

    [HttpPut("users/{id:int}/status")]
    public async Task<IActionResult> UpdateUserStatus(int id, [FromBody] UpdateUserStatusRequest request)
    {
        if (!IsAdmin) return Forbid();

        var user = await userDb.Users.FindAsync(id);
        if (user == null) return NotFound(ApiResponse<object>.Fail("User not found."));

        user.Status = request.Status;
        await userDb.SaveChangesAsync();

        return Ok(ApiResponse<object>.Ok(null, "User status updated."));
    }

    [HttpPut("users/{id:int}/points")]
    public async Task<IActionResult> UpdateUserPoints(int id, [FromBody] int points)
    {
        if (!IsAdmin) return Forbid();

        var user = await userDb.Users.FindAsync(id);
        if (user == null) return NotFound(ApiResponse<object>.Fail("User not found."));

        user.Point += points;
        await userDb.SaveChangesAsync();

        return Ok(ApiResponse<object>.Ok(new { user.Point }, "Points updated."));
    }

    [HttpGet("news")]
    public async Task<IActionResult> GetAllNews([FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        if (!IsAdmin) return Forbid();

        var total = await db.News.CountAsync();
        var news = await db.News
            .OrderByDescending(n => n.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return Ok(ApiResponse<PagedResult<News>>.Ok(new PagedResult<News>
        {
            Items = news,
            Total = total,
            Page = page,
            PageSize = pageSize
        }));
    }

    [HttpPost("news")]
    public async Task<IActionResult> CreateNews([FromBody] CreateNewsRequest request)
    {
        if (!IsAdmin) return Forbid();

        var news = new News
        {
            Title = request.Title,
            Content = request.Content,
            IsPublished = request.IsPublished,
            Author = request.Author,
            CreatedAt = DateTime.UtcNow
        };

        db.News.Add(news);
        await db.SaveChangesAsync();

        return Ok(ApiResponse<News>.Ok(news));
    }

    [HttpPut("news/{id:int}")]
    public async Task<IActionResult> UpdateNews(int id, [FromBody] CreateNewsRequest request)
    {
        if (!IsAdmin) return Forbid();

        var news = await db.News.FindAsync(id);
        if (news == null) return NotFound(ApiResponse<object>.Fail("News not found."));

        news.Title = request.Title;
        news.Content = request.Content;
        news.IsPublished = request.IsPublished;
        news.UpdatedAt = DateTime.UtcNow;

        await db.SaveChangesAsync();
        return Ok(ApiResponse<News>.Ok(news));
    }

    [HttpDelete("news/{id:int}")]
    public async Task<IActionResult> DeleteNews(int id)
    {
        if (!IsAdmin) return Forbid();

        var news = await db.News.FindAsync(id);
        if (news == null) return NotFound(ApiResponse<object>.Fail("News not found."));

        db.News.Remove(news);
        await db.SaveChangesAsync();

        return Ok(ApiResponse<object>.Ok(null, "News deleted."));
    }

    [HttpGet("settings")]
    public async Task<IActionResult> GetSettings()
    {
        if (!IsAdmin) return Forbid();

        var settings = await db.Settings.ToListAsync();
        return Ok(ApiResponse<List<Setting>>.Ok(settings));
    }

    [HttpPut("settings/{key}")]
    public async Task<IActionResult> UpdateSetting(string key, [FromBody] string value)
    {
        if (!IsAdmin) return Forbid();

        var setting = await db.Settings.FirstOrDefaultAsync(s => s.Key == key);
        if (setting == null)
        {
            setting = new Setting { Key = key, Value = value };
            db.Settings.Add(setting);
        }
        else
        {
            setting.Value = value;
        }

        await db.SaveChangesAsync();
        return Ok(ApiResponse<Setting>.Ok(setting));
    }

    [HttpGet("redeem-codes")]
    public async Task<IActionResult> GetRedeemCodes([FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        if (!IsAdmin) return Forbid();

        var total = await db.RedeemCodes.CountAsync();
        var codes = await db.RedeemCodes
            .OrderByDescending(c => c.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return Ok(ApiResponse<PagedResult<RedeemCode>>.Ok(new PagedResult<RedeemCode>
        {
            Items = codes,
            Total = total,
            Page = page,
            PageSize = pageSize
        }));
    }

    [HttpPost("redeem-codes")]
    public async Task<IActionResult> CreateRedeemCode([FromBody] CreateRedeemCodeRequest request)
    {
        if (!IsAdmin) return Forbid();

        var code = new RedeemCode
        {
            Code = request.Code,
            Description = request.Description,
            MaxUses = request.MaxUses,
            ExpiresAt = request.ExpiresAt,
            Active = true,
            CreatedAt = DateTime.UtcNow
        };

        db.RedeemCodes.Add(code);
        await db.SaveChangesAsync();

        foreach (var item in request.Items)
        {
            db.RedeemCodeItems.Add(new RedeemCodeItem
            {
                RedeemCodeId = code.Id,
                ItemType = item.ItemType,
                TypeId = item.TypeId,
                Amount = item.Amount,
                Points = item.Points
            });
        }

        await db.SaveChangesAsync();
        return Ok(ApiResponse<RedeemCode>.Ok(code));
    }

    [HttpDelete("redeem-codes/{id:int}")]
    public async Task<IActionResult> DeleteRedeemCode(int id)
    {
        if (!IsAdmin) return Forbid();

        var code = await db.RedeemCodes.FindAsync(id);
        if (code == null) return NotFound(ApiResponse<object>.Fail("Code not found."));

        code.Active = false;
        await db.SaveChangesAsync();

        return Ok(ApiResponse<object>.Ok(null, "Code deactivated."));
    }

    [HttpGet("shop-items")]
    public async Task<IActionResult> GetShopItems([FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        if (!IsAdmin) return Forbid();

        var total = await db.ShopItems.CountAsync();
        var items = await db.ShopItems
            .OrderBy(i => i.SortOrder)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return Ok(ApiResponse<PagedResult<ShopItem>>.Ok(new PagedResult<ShopItem>
        {
            Items = items,
            Total = total,
            Page = page,
            PageSize = pageSize
        }));
    }

    [HttpGet("boss-kills")]
    public async Task<IActionResult> GetBossKills([FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        if (!IsAdmin) return Forbid();

        var total = await gameLogDb.BossDeathLogs.CountAsync();
        var logs = await gameLogDb.BossDeathLogs
            .OrderByDescending(l => l.KillTime)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return Ok(ApiResponse<PagedResult<BossDeathLog>>.Ok(new PagedResult<BossDeathLog>
        {
            Items = logs,
            Total = total,
            Page = page,
            PageSize = pageSize
        }));
    }
}
