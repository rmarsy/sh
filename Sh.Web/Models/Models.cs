namespace Sh.Web.Models;

public class ApiResponse<T>
{
    public bool Success { get; set; }
    public string? Message { get; set; }
    public T? Data { get; set; }
}

public class PagedResult<T>
{
    public List<T> Items { get; set; } = [];
    public int Total { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
}

public class LoginResponse
{
    public string Token { get; set; } = string.Empty;
    public UserDto User { get; set; } = new();
}

public class UserDto
{
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public bool Admin { get; set; }
    public int AdminLevel { get; set; }
    public int Point { get; set; }
    public int Vp { get; set; }
    public short Status { get; set; }
    public DateTime JoinDate { get; set; }
}

public class AdminUserDto : UserDto
{
    public string? UserIp { get; set; }
    public int Leave { get; set; }
    public DateTime? LeaveDate { get; set; }
}

public class CharacterDto
{
    public int CharId { get; set; }
    public short ServerId { get; set; }
    public string CharName { get; set; } = string.Empty;
    public short Family { get; set; }
    public short Grow { get; set; }
    public short Job { get; set; }
    public short Sex { get; set; }
    public short Level { get; set; }
    public int K1 { get; set; }
    public long Money { get; set; }
    public bool LoginStatus { get; set; }
    public short Map { get; set; }
}

public class DashboardDto
{
    public UserDto User { get; set; } = new();
    public List<CharacterDto> Characters { get; set; } = [];
}

public class HomeDto
{
    public List<NewsDto> LatestNews { get; set; } = [];
    public List<CharacterDto> PvpRanking { get; set; } = [];
    public ServerStatusDto ServerStatus { get; set; } = new();
}

public class ServerStatusDto
{
    public bool Online { get; set; }
    public int Players { get; set; }
}

public class NewsDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string? ImageUrl { get; set; }
    public string? Slug { get; set; }
    public bool Published { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class ShopDto
{
    public List<ShopCategoryDto> Categories { get; set; } = [];
    public List<ShopItemDto> Items { get; set; } = [];
}

public class ShopCategoryDto
{
    public int Id { get; set; }
    public int ShopId { get; set; }
    public string Name { get; set; } = string.Empty;
    public int SortOrder { get; set; }
    public bool Active { get; set; }
}

public class ShopItemDto
{
    public int Id { get; set; }
    public int CategoryId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }
    public int Price { get; set; }
    public string Currency { get; set; } = "point";
    public short ItemType { get; set; }
    public short TypeId { get; set; }
    public int Amount { get; set; }
    public bool Active { get; set; }
    public bool Featured { get; set; }
    public int SortOrder { get; set; }
}

public class PurchaseResult
{
    public int NewBalance { get; set; }
}

public class RouletteDto
{
    public RouletteInfo Info { get; set; } = new();
    public List<RoulettePrizeDto> Prizes { get; set; } = [];
}

public class RouletteInfo
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int CostPerSpin { get; set; }
    public string Currency { get; set; } = "point";
    public bool Active { get; set; }
}

public class RoulettePrizeDto
{
    public int Id { get; set; }
    public int RouletteId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? ImageUrl { get; set; }
    public short ItemType { get; set; }
    public short TypeId { get; set; }
    public int Amount { get; set; }
    public int Points { get; set; }
    public int Weight { get; set; }
}

public class SpinResultDto
{
    public RoulettePrizeDto Prize { get; set; } = new();
    public int NewBalance { get; set; }
}

public class VoteDto
{
    public bool CanVote { get; set; }
    public DateTime? NextVoteTime { get; set; }
    public List<VoteSiteDto> Sites { get; set; } = [];
}

public class VoteSiteDto
{
    public string Name { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
    public int PointsReward { get; set; }
    public bool Voted { get; set; }
    public DateTime? NextVoteTime { get; set; }
}

public class VoteClaimResult
{
    public int NewBalance { get; set; }
}

public class BattlePassDto
{
    public List<BattlePassLevelDto> Levels { get; set; } = [];
    public List<BattlePassTaskDto> Tasks { get; set; } = [];
    public BattlePassUserProgressDto? UserProgress { get; set; }
    public int Season { get; set; }
}

public class BattlePassLevelDto
{
    public int Id { get; set; }
    public int Season { get; set; }
    public int Level { get; set; }
    public int PointsRequired { get; set; }
    public string RewardName { get; set; } = string.Empty;
    public string? RewardImageUrl { get; set; }
    public short ItemType { get; set; }
    public short TypeId { get; set; }
    public int Amount { get; set; }
}

public class BattlePassTaskDto
{
    public int Id { get; set; }
    public int Season { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int PointsReward { get; set; }
    public bool Active { get; set; }
}

public class BattlePassUserProgressDto
{
    public int Id { get; set; }
    public int UserUid { get; set; }
    public int Season { get; set; }
    public int Points { get; set; }
    public int CurrentLevel { get; set; }
}

public class PaymentDto
{
    public int Id { get; set; }
    public string Provider { get; set; } = string.Empty;
    public string? TransactionId { get; set; }
    public decimal Amount { get; set; }
    public string Currency { get; set; } = "USD";
    public int PointsAwarded { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
}

public class ProfileDto
{
    public string UserId { get; set; } = string.Empty;
    public string? SecurityQuestion { get; set; }
    public DateTime JoinDate { get; set; }
    public int Point { get; set; }
    public int Vp { get; set; }
}

public class RedeemCodeDto
{
    public int Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int MaxUses { get; set; }
    public int CurrentUses { get; set; }
    public DateTime? ExpiresAt { get; set; }
    public bool Active { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class SettingDto
{
    public int Id { get; set; }
    public string Key { get; set; } = string.Empty;
    public string? Value { get; set; }
    public string? Description { get; set; }
}
