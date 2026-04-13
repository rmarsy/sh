using Microsoft.EntityFrameworkCore;
using Sh.Api.Data;
using Sh.Api.Models;

namespace Sh.Api.Services;

public class ServerManager(GameDbContext gameDb)
{
    public async Task<ServerStatusDto> GetStatusAsync()
    {
        try
        {
            var onlinePlayers = await gameDb.Characters
                .Where(c => c.LoginStatus && c.Del == 0)
                .CountAsync();

            return new ServerStatusDto
            {
                Online = true,
                Players = onlinePlayers
            };
        }
        catch
        {
            return new ServerStatusDto { Online = false, Players = 0 };
        }
    }
}
