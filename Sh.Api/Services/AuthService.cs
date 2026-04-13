using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Sh.Api.Data;
using Sh.Api.Models;

namespace Sh.Api.Services;

public class AuthService(UserDbContext userDb, IConfiguration configuration)
{
    public async Task<User?> ValidateUserAsync(string username, string password)
    {
        var user = await userDb.Users
            .FirstOrDefaultAsync(u => u.UserId == username);

        if (user == null) return null;
        if (!BCrypt.Net.BCrypt.Verify(password, user.Pw)) return null;
        return user;
    }

    public async Task<(bool Success, string Message)> RegisterAsync(string username, string password, string? ip)
    {
        if (await userDb.Users.AnyAsync(u => u.UserId == username))
            return (false, "Username already taken.");

        var user = new User
        {
            UserId = username,
            Pw = BCrypt.Net.BCrypt.HashPassword(password),
            JoinDate = DateTime.UtcNow,
            Status = 0,
            UserType = "N",
            UserIp = ip,
            Point = 0,
            Vp = 0
        };

        userDb.Users.Add(user);
        await userDb.SaveChangesAsync();
        return (true, "Registration successful.");
    }

    public async Task<(bool Success, string Message, string? Token)> GenerateResetTokenAsync(string username, string securityAnswer)
    {
        var user = await userDb.Users.FirstOrDefaultAsync(u =>
            u.UserId == username && u.SecurityAnswer != null && u.SecurityAnswer == securityAnswer.ToLower());

        if (user == null)
            return (false, "No account found with those credentials.", null);

        var token = Guid.NewGuid().ToString("N") + Guid.NewGuid().ToString("N");
        user.ResetToken = token;
        user.ResetTokenExpiresAt = DateTime.UtcNow.AddHours(1);
        await userDb.SaveChangesAsync();

        return (true, "Reset token generated.", token);
    }

    public async Task<(bool Success, string Message)> ResetPasswordAsync(string token, string newPassword)
    {
        var user = await userDb.Users.FirstOrDefaultAsync(u =>
            u.ResetToken == token && u.ResetTokenExpiresAt > DateTime.UtcNow);

        if (user == null)
            return (false, "Invalid or expired reset token.");

        user.Pw = BCrypt.Net.BCrypt.HashPassword(newPassword);
        user.ResetToken = null;
        user.ResetTokenExpiresAt = null;
        await userDb.SaveChangesAsync();

        return (true, "Password reset successfully.");
    }

    public string GenerateJwtToken(User user)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expiry = DateTime.UtcNow.AddMinutes(configuration.GetValue<int>("Jwt:ExpiryMinutes"));

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.UserUid.ToString()),
            new Claim(ClaimTypes.Name, user.UserId),
            new Claim("admin", user.Admin.ToString().ToLower()),
            new Claim("adminlevel", user.AdminLevel.ToString()),
            new Claim("point", user.Point.ToString()),
            new Claim("vp", user.Vp.ToString())
        };

        var token = new JwtSecurityToken(
            issuer: configuration["Jwt:Issuer"],
            audience: configuration["Jwt:Audience"],
            claims: claims,
            expires: expiry,
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public UserDto MapToDto(User user) => new()
    {
        Id = user.UserUid,
        Username = user.UserId,
        Admin = user.Admin,
        AdminLevel = user.AdminLevel,
        Point = user.Point,
        Vp = user.Vp,
        Status = user.Status,
        JoinDate = user.JoinDate
    };
}
