using Microsoft.EntityFrameworkCore;
using Sh.Api.Data;
using Sh.Api.Models;

namespace Sh.Api.Services;

public class ShopService(AppDbContext db, UserDbContext userDb)
{
    public async Task<ShopDto> GetShopAsync()
    {
        var categories = await db.ShopCategories.Where(c => c.Active).OrderBy(c => c.SortOrder).ToListAsync();
        var items = await db.ShopItems.Where(i => i.Active).OrderBy(i => i.SortOrder).ToListAsync();
        return new ShopDto { Categories = categories, Items = items };
    }

    public async Task<(bool Success, string Message, int NewBalance)> PurchaseAsync(int userUid, int itemId, int characterId, int amount)
    {
        var item = await db.ShopItems.FindAsync(itemId);
        if (item == null || !item.Active) return (false, "Item not found.", 0);

        var user = await userDb.Users.FindAsync(userUid);
        if (user == null) return (false, "User not found.", 0);

        var totalCost = item.Price * amount;

        if (item.Currency == "point")
        {
            if (user.Point < totalCost) return (false, "Insufficient points.", user.Point);
            user.Point -= totalCost;
        }
        else if (item.Currency == "vp")
        {
            if (user.Vp < totalCost) return (false, "Insufficient vote points.", user.Vp);
            user.Vp -= totalCost;
        }

        var purchase = new ShopPurchase
        {
            UserUid = userUid,
            ItemId = itemId,
            CharacterId = characterId,
            Amount = amount,
            PointSpent = totalCost,
            PurchasedAt = DateTime.UtcNow
        };

        db.ShopPurchases.Add(purchase);
        await db.SaveChangesAsync();
        await userDb.SaveChangesAsync();

        return (true, "Purchase successful.", item.Currency == "point" ? user.Point : user.Vp);
    }
}
