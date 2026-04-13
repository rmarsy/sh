using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Sh.Web.Models;

namespace Sh.Web.Services;

public class AuthStateProvider(IHttpContextAccessor httpContextAccessor, ApiService apiService)
{
    public UserDto? CurrentUser { get; private set; }
    public bool IsAuthenticated => httpContextAccessor.HttpContext?.User.Identity?.IsAuthenticated ?? false;
    public bool IsAdmin => IsAuthenticated && CurrentUser?.Admin == true;

    public async Task<(bool Success, string Message)> LoginAsync(string username, string password, string? turnstileToken)
    {
        var (success, message, data) = await apiService.LoginAsync(username, password, turnstileToken);
        if (!success || data == null) return (false, message);

        var ctx = httpContextAccessor.HttpContext!;

        ctx.Response.Cookies.Append("sh_token", data.Token, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Expires = DateTimeOffset.UtcNow.AddMinutes(120)
        });

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, data.User.Id.ToString()),
            new(ClaimTypes.Name, data.User.Username),
            new("admin", data.User.Admin.ToString().ToLower()),
            new("adminlevel", data.User.AdminLevel.ToString()),
            new("point", data.User.Point.ToString()),
            new("vp", data.User.Vp.ToString())
        };

        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var principal = new ClaimsPrincipal(identity);

        await ctx.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal,
            new AuthenticationProperties { IsPersistent = true, ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(120) });

        CurrentUser = data.User;
        return (true, message);
    }

    public async Task LogoutAsync()
    {
        var ctx = httpContextAccessor.HttpContext!;
        ctx.Response.Cookies.Delete("sh_token");
        await ctx.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        CurrentUser = null;
    }

    public UserDto? GetCurrentUser()
    {
        var ctx = httpContextAccessor.HttpContext;
        if (ctx?.User.Identity?.IsAuthenticated != true) return null;

        return new UserDto
        {
            Id = int.TryParse(ctx.User.FindFirstValue(ClaimTypes.NameIdentifier), out var id) ? id : 0,
            Username = ctx.User.FindFirstValue(ClaimTypes.Name) ?? string.Empty,
            Admin = ctx.User.FindFirstValue("admin") == "true",
            AdminLevel = int.TryParse(ctx.User.FindFirstValue("adminlevel"), out var al) ? al : 0,
            Point = int.TryParse(ctx.User.FindFirstValue("point"), out var p) ? p : 0,
            Vp = int.TryParse(ctx.User.FindFirstValue("vp"), out var vp) ? vp : 0
        };
    }
}
