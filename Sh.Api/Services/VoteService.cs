using Microsoft.EntityFrameworkCore;
using Sh.Api.Data;
using Sh.Api.Models;

namespace Sh.Api.Services;

public class VoteService(AppDbContext db, UserDbContext userDb)
{
    private static readonly List<VoteSiteConfig> VoteSites =
    [
        new("GTop100", "https://gtop100.com/vote", 5),
        new("Xtremetop100", "https://xtremetop100.com/vote", 5)
    ];

    public async Task<VoteDto> GetVoteStatusAsync(int userUid)
    {
        var now = DateTime.UtcNow;
        var sites = new List<VoteSiteDto>();

        foreach (var site in VoteSites)
        {
            var lastVote = await db.Votes
                .Where(v => v.UserUid == userUid && v.VoteSite == site.Name)
                .OrderByDescending(v => v.VotedAt)
                .FirstOrDefaultAsync();

            var canVote = lastVote == null || lastVote.VotedAt.AddHours(12) < now;
            var nextVoteTime = lastVote != null && !canVote ? lastVote.VotedAt.AddHours(12) : (DateTime?)null;

            sites.Add(new VoteSiteDto
            {
                Name = site.Name,
                Url = site.Url,
                PointsReward = site.PointsReward,
                Voted = !canVote,
                NextVoteTime = nextVoteTime
            });
        }

        var canVoteAny = sites.Any(s => !s.Voted);
        return new VoteDto
        {
            CanVote = canVoteAny,
            Sites = sites
        };
    }

    public async Task<(bool Success, string Message, int NewBalance)> ClaimVoteAsync(int userUid, string voteSite)
    {
        var site = VoteSites.FirstOrDefault(s => s.Name == voteSite);
        if (site == null) return (false, "Unknown vote site.", 0);

        var now = DateTime.UtcNow;
        var lastVote = await db.Votes
            .Where(v => v.UserUid == userUid && v.VoteSite == voteSite)
            .OrderByDescending(v => v.VotedAt)
            .FirstOrDefaultAsync();

        if (lastVote != null && lastVote.VotedAt.AddHours(12) > now)
            return (false, "You must wait before voting again.", 0);

        var user = await userDb.Users.FindAsync(userUid);
        if (user == null) return (false, "User not found.", 0);

        user.Vp += site.PointsReward;

        var vote = new Vote
        {
            UserUid = userUid,
            VoteSite = voteSite,
            VotedAt = now,
            PointsAwarded = site.PointsReward
        };

        db.Votes.Add(vote);
        await db.SaveChangesAsync();
        await userDb.SaveChangesAsync();

        return (true, $"You received {site.PointsReward} vote points!", user.Vp);
    }

    public async Task<(bool Success, string Message, int NewBalance)> ProcessExternalVoteAsync(int userUid, string voteSite, string? ip, string? secret)
    {
        var site = VoteSites.FirstOrDefault(s => s.Name == voteSite);
        if (site == null) throw new InvalidOperationException("Unknown vote site.");

        var now = DateTime.UtcNow;
        var lastVote = await db.Votes
            .Where(v => v.UserUid == userUid && v.VoteSite == voteSite)
            .OrderByDescending(v => v.VotedAt)
            .FirstOrDefaultAsync();

        if (lastVote != null && lastVote.VotedAt.AddHours(12) > now)
            throw new InvalidOperationException("Vote cooldown not elapsed.");

        var user = await userDb.Users.FindAsync(userUid);
        if (user == null) throw new InvalidOperationException("User not found.");

        user.Vp += site.PointsReward;

        var vote = new Vote
        {
            UserUid = userUid,
            VoteSite = voteSite,
            VotedAt = now,
            PointsAwarded = site.PointsReward
        };

        db.Votes.Add(vote);
        await db.SaveChangesAsync();
        await userDb.SaveChangesAsync();

        return (true, $"Vote processed for {voteSite}.", user.Vp);
    }

    private record VoteSiteConfig(string Name, string Url, int PointsReward);
}
