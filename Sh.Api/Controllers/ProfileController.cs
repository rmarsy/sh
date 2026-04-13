using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sh.Api.Data;
using Sh.Api.Models;

namespace Sh.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ProfileController(UserDbContext userDb) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetProfile()
    {
        var userUid = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var user = await userDb.Users.FindAsync(userUid);
        if (user == null) return Unauthorized();

        return Ok(ApiResponse<object>.Ok(new
        {
            user.UserId,
            user.SecurityQuestion,
            user.JoinDate,
            user.Point,
            user.Vp
        }));
    }

    [HttpPut("password")]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
    {
        var userUid = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var user = await userDb.Users.FindAsync(userUid);
        if (user == null) return Unauthorized();

        if (!BCrypt.Net.BCrypt.Verify(request.CurrentPassword, user.Pw))
            return BadRequest(ApiResponse<object>.Fail("Current password is incorrect."));

        user.Pw = BCrypt.Net.BCrypt.HashPassword(request.NewPassword);
        await userDb.SaveChangesAsync();

        return Ok(ApiResponse<object>.Ok(null, "Password changed successfully."));
    }

    [HttpPut("security-question")]
    public async Task<IActionResult> SetSecurityQuestion([FromBody] SecurityQuestionRequest request)
    {
        var userUid = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var user = await userDb.Users.FindAsync(userUid);
        if (user == null) return Unauthorized();

        user.SecurityQuestion = request.Question;
        user.SecurityAnswer = request.Answer;
        await userDb.SaveChangesAsync();

        return Ok(ApiResponse<object>.Ok(null, "Security question updated."));
    }
}
