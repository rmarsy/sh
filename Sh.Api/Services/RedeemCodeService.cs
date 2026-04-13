using Microsoft.EntityFrameworkCore;
using Sh.Api.Data;
using Sh.Api.Models;

namespace Sh.Api.Services;

public class RedeemCodeService(AppDbContext db, UserDbContext userDb)
{
    public async Task<(bool Success, string Message)> RedeemAsync(int userUid, string code, int characterId)
    {
        var redeemCode = await db.RedeemCodes
            .FirstOrDefaultAsync(r => r.Code == code && r.Active);

        if (redeemCode == null) return (false, "Invalid or inactive code.");

        if (redeemCode.ExpiresAt.HasValue && redeemCode.ExpiresAt < DateTime.UtcNow)
            return (false, "Code has expired.");

        if (redeemCode.MaxUses > 0 && redeemCode.CurrentUses >= redeemCode.MaxUses)
            return (false, "Code has reached its maximum uses.");

        var alreadyUsed = await db.RedeemCodeLogs.AnyAsync(l =>
            l.RedeemCodeId == redeemCode.Id && l.UserUid == userUid);

        if (alreadyUsed) return (false, "You have already used this code.");

        var user = await userDb.Users.FindAsync(userUid);
        if (user == null) return (false, "User not found.");

        var items = await db.RedeemCodeItems
            .Where(i => i.RedeemCodeId == redeemCode.Id)
            .ToListAsync();

        foreach (var item in items)
        {
            if (item.Points > 0) user.Point += item.Points;
        }

        redeemCode.CurrentUses++;

        var log = new RedeemCodeLog
        {
            RedeemCodeId = redeemCode.Id,
            UserUid = userUid,
            RedeemedAt = DateTime.UtcNow
        };

        db.RedeemCodeLogs.Add(log);
        await db.SaveChangesAsync();
        await userDb.SaveChangesAsync();

        return (true, "Code redeemed successfully.");
    }
}
