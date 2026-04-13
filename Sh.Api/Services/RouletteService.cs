using Microsoft.EntityFrameworkCore;
using Sh.Api.Data;
using Sh.Api.Models;

namespace Sh.Api.Services;

public class RouletteService(AppDbContext db, UserDbContext userDb)
{
    public async Task<RouletteDto?> GetActiveRouletteAsync()
    {
        var roulette = await db.Roulettes.FirstOrDefaultAsync(r => r.Active);
        if (roulette == null) return null;

        var prizes = await db.RoulettePrizes.Where(p => p.RouletteId == roulette.Id).ToListAsync();
        return new RouletteDto { Info = roulette, Prizes = prizes };
    }

    public async Task<(bool Success, string Message, SpinResultDto? Result)> SpinAsync(int userUid, int rouletteId, int characterId)
    {
        var roulette = await db.Roulettes.FindAsync(rouletteId);
        if (roulette == null || !roulette.Active) return (false, "Roulette not found.", null);

        var user = await userDb.Users.FindAsync(userUid);
        if (user == null) return (false, "User not found.", null);

        if (roulette.Currency == "point" && user.Point < roulette.CostPerSpin)
            return (false, "Insufficient points.", null);
        if (roulette.Currency == "vp" && user.Vp < roulette.CostPerSpin)
            return (false, "Insufficient vote points.", null);

        var prizes = await db.RoulettePrizes.Where(p => p.RouletteId == rouletteId).ToListAsync();
        if (prizes.Count == 0) return (false, "No prizes configured.", null);

        var prize = SelectPrize(prizes);

        if (roulette.Currency == "point") user.Point -= roulette.CostPerSpin;
        else if (roulette.Currency == "vp") user.Vp -= roulette.CostPerSpin;

        if (prize.Points > 0) user.Point += prize.Points;

        var history = new RouletteHistory
        {
            RouletteId = rouletteId,
            UserUid = userUid,
            PrizeId = prize.Id,
            CharacterId = characterId,
            SpunAt = DateTime.UtcNow
        };

        db.RouletteHistories.Add(history);
        await db.SaveChangesAsync();
        await userDb.SaveChangesAsync();

        return (true, "Spin successful.", new SpinResultDto
        {
            Prize = prize,
            NewBalance = roulette.Currency == "point" ? user.Point : user.Vp
        });
    }

    private static RoulettePrize SelectPrize(List<RoulettePrize> prizes)
    {
        var totalWeight = prizes.Sum(p => p.Weight);
        var roll = Random.Shared.Next(0, totalWeight);
        var cumulative = 0;

        foreach (var prize in prizes)
        {
            cumulative += prize.Weight;
            if (roll < cumulative) return prize;
        }

        return prizes.Last();
    }
}
