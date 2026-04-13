using Microsoft.EntityFrameworkCore;
using Sh.Api.Models;

namespace Sh.Api.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<News> News => Set<News>();
    public DbSet<Shop> Shops => Set<Shop>();
    public DbSet<ShopItem> ShopItems => Set<ShopItem>();
    public DbSet<ShopCategory> ShopCategories => Set<ShopCategory>();
    public DbSet<ShopPurchase> ShopPurchases => Set<ShopPurchase>();
    public DbSet<RedeemCode> RedeemCodes => Set<RedeemCode>();
    public DbSet<RedeemCodeItem> RedeemCodeItems => Set<RedeemCodeItem>();
    public DbSet<RedeemCodeLog> RedeemCodeLogs => Set<RedeemCodeLog>();
    public DbSet<Roulette> Roulettes => Set<Roulette>();
    public DbSet<RoulettePrize> RoulettePrizes => Set<RoulettePrize>();
    public DbSet<RouletteHistory> RouletteHistories => Set<RouletteHistory>();
    public DbSet<Vote> Votes => Set<Vote>();
    public DbSet<BattlePassLevel> BattlePassLevels => Set<BattlePassLevel>();
    public DbSet<BattlePassTask> BattlePassTasks => Set<BattlePassTask>();
    public DbSet<BattlePassUserPoints> BattlePassUserPoints => Set<BattlePassUserPoints>();
    public DbSet<Payment> Payments => Set<Payment>();
    public DbSet<Setting> Settings => Set<Setting>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<News>().ToTable("News");
        modelBuilder.Entity<Shop>().ToTable("Shop");
        modelBuilder.Entity<ShopItem>().ToTable("ShopItem");
        modelBuilder.Entity<ShopCategory>().ToTable("ShopCategory");
        modelBuilder.Entity<ShopPurchase>().ToTable("ShopPurchase");
        modelBuilder.Entity<RedeemCode>().ToTable("RedeemCodes");
        modelBuilder.Entity<RedeemCodeItem>().ToTable("RedeemCodeItems");
        modelBuilder.Entity<RedeemCodeLog>().ToTable("RedeemCodeLogs");
        modelBuilder.Entity<Roulette>().ToTable("Roulette");
        modelBuilder.Entity<RoulettePrize>().ToTable("RoulettePrizes");
        modelBuilder.Entity<RouletteHistory>().ToTable("RouletteHistory");
        modelBuilder.Entity<Vote>().ToTable("Vote");
        modelBuilder.Entity<BattlePassLevel>().ToTable("BattlePassLevels");
        modelBuilder.Entity<BattlePassTask>().ToTable("BattlePassTasks");
        modelBuilder.Entity<BattlePassUserPoints>().ToTable("BattlePassUserPoints");
        modelBuilder.Entity<Payment>().ToTable("Payments");
        modelBuilder.Entity<Setting>().ToTable("Settings");
    }
}

public class UserDbContext(DbContextOptions<UserDbContext> options) : DbContext(options)
{
    public DbSet<User> Users => Set<User>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().ToTable("Users_master");
    }
}

public class GameDbContext(DbContextOptions<GameDbContext> options) : DbContext(options)
{
    public DbSet<Character> Characters => Set<Character>();
    public DbSet<GameItem> Items => Set<GameItem>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Character>().ToTable("Chars");
        modelBuilder.Entity<GameItem>().ToTable("Items");
    }
}

public class GameLogDbContext(DbContextOptions<GameLogDbContext> options) : DbContext(options)
{
    public DbSet<BossDeathLog> BossDeathLogs => Set<BossDeathLog>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BossDeathLog>().ToTable("Boss_Death_Log");
    }
}
