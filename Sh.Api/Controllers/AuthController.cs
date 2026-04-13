using Microsoft.AspNetCore.RateLimiting;
using Microsoft.AspNetCore.Mvc;
using Sh.Api.Models;
using Sh.Api.Services;

namespace Sh.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[EnableRateLimiting("auth")]
public class AuthController(AuthService authService, TurnstileService turnstileService, IHttpContextAccessor httpContextAccessor) : ControllerBase
{
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        if (!ModelState.IsValid) return BadRequest(ApiResponse<object>.Fail("Invalid request."));

        if (!string.IsNullOrEmpty(request.TurnstileToken))
        {
            var ip = httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString();
            var valid = await turnstileService.VerifyAsync(request.TurnstileToken, ip);
            if (!valid) return BadRequest(ApiResponse<object>.Fail("CAPTCHA verification failed."));
        }

        var user = await authService.ValidateUserAsync(request.Username, request.Password);
        if (user == null)
            return Unauthorized(ApiResponse<object>.Fail("Invalid username or password."));

        if (user.Status == -5)
            return Unauthorized(ApiResponse<object>.Fail("Your account has been banned."));

        var token = authService.GenerateJwtToken(user);
        var dto = authService.MapToDto(user);

        Response.Cookies.Append("sh_token", token, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Expires = DateTimeOffset.UtcNow.AddMinutes(120)
        });

        return Ok(ApiResponse<LoginResponse>.Ok(new LoginResponse { Token = token, User = dto }));
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        if (!ModelState.IsValid) return BadRequest(ApiResponse<object>.Fail("Invalid request."));

        var ip = httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString();
        var valid = await turnstileService.VerifyAsync(request.TurnstileToken, ip);
        if (!valid) return BadRequest(ApiResponse<object>.Fail("CAPTCHA verification failed."));

        var (success, message) = await authService.RegisterAsync(request.Username, request.Password, request.Email, ip);
        if (!success) return BadRequest(ApiResponse<object>.Fail(message));

        return Ok(ApiResponse<object>.Ok(null, message));
    }

    [HttpPost("forgot-password")]
    public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest request)
    {
        if (!ModelState.IsValid) return BadRequest(ApiResponse<object>.Fail("Invalid request."));

        var ip = httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString();
        var valid = await turnstileService.VerifyAsync(request.TurnstileToken, ip);
        if (!valid) return BadRequest(ApiResponse<object>.Fail("CAPTCHA verification failed."));

        await authService.GenerateResetTokenAsync(request.Username, request.Email);
        return Ok(ApiResponse<object>.Ok(null, "If an account was found, a reset link has been sent."));
    }

    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
    {
        if (!ModelState.IsValid) return BadRequest(ApiResponse<object>.Fail("Invalid request."));

        var (success, message) = await authService.ResetPasswordAsync(request.Token, request.NewPassword);
        if (!success) return BadRequest(ApiResponse<object>.Fail(message));

        return Ok(ApiResponse<object>.Ok(null, message));
    }

    [HttpPost("logout")]
    public IActionResult Logout()
    {
        Response.Cookies.Delete("sh_token");
        return Ok(ApiResponse<object>.Ok(null, "Logged out."));
    }
}
