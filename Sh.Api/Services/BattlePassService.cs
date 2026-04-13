using Microsoft.EntityFrameworkCore;
using Sh.Api.Data;
using Sh.Api.Models;

namespace Sh.Api.Services;

public class BattlePassService(AppDbContext db)
{
    private const int CurrentSeason = 1;

    public async Task<BattlePassDto> GetBattlePassAsync(int userUid)
    {
        var levels = await db.BattlePassLevels
            .Where(l => l.Season == CurrentSeason)
            .OrderBy(l => l.Level)
            .ToListAsync();

        var tasks = await db.BattlePassTasks
            .Where(t => t.Season == CurrentSeason && t.Active)
            .ToListAsync();

        var userProgress = await db.BattlePassUserPoints
            .FirstOrDefaultAsync(u => u.UserUid == userUid && u.Season == CurrentSeason);

        return new BattlePassDto
        {
            Levels = levels,
            Tasks = tasks,
            UserProgress = userProgress,
            Season = CurrentSeason
        };
    }
}
