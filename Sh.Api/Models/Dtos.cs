namespace Sh.Api.Models;

public record LoginRequest(string Username, string Password, string? TurnstileToken);
public record RegisterRequest(string Username, string Password, string Email, string TurnstileToken);
public record ForgotPasswordRequest(string Username, string Email, string TurnstileToken);
public record ResetPasswordRequest(string Token, string NewPassword);
public record ChangePasswordRequest(string CurrentPassword, string NewPassword);
public record SecurityQuestionRequest(string Question, string Answer);
public record PurchaseRequest(int ItemId, int CharacterId, int Amount);
public record RouletteSpinRequest(int RouletteId, int CharacterId);
public record RedeemRequest(string Code, int CharacterId);
public record VoteClaimRequest(string VoteSite);
public record DonateRequest(string Provider, decimal Amount, string Currency);

public class UserDto
{
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string? Email { get; set; }
    public bool Admin { get; set; }
    public int AdminLevel { get; set; }
    public int Point { get; set; }
    public int Vp { get; set; }
    public short Status { get; set; }
    public DateTime JoinDate { get; set; }
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

public class LoginResponse
{
    public string Token { get; set; } = string.Empty;
    public UserDto User { get; set; } = new();
}

public class ApiResponse<T>
{
    public bool Success { get; set; }
    public string? Message { get; set; }
    public T? Data { get; set; }

    public static ApiResponse<T> Ok(T? data, string? message = null) =>
        new() { Success = true, Data = data, Message = message };

    public static ApiResponse<T> Fail(string message) =>
        new() { Success = false, Message = message };
}

public class PagedResult<T>
{
    public List<T> Items { get; set; } = [];
    public int Total { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
}

public class HomeDto
{
    public List<News> LatestNews { get; set; } = [];
    public List<CharacterDto> PvpRanking { get; set; } = [];
    public ServerStatusDto ServerStatus { get; set; } = new();
}

public class ServerStatusDto
{
    public bool Online { get; set; }
    public int Players { get; set; }
}

public class DashboardDto
{
    public UserDto User { get; set; } = new();
    public List<CharacterDto> Characters { get; set; } = [];
}

public class ShopDto
{
    public List<ShopCategory> Categories { get; set; } = [];
    public List<ShopItem> Items { get; set; } = [];
}

public class RouletteDto
{
    public Roulette Info { get; set; } = new();
    public List<RoulettePrize> Prizes { get; set; } = [];
}

public class SpinResultDto
{
    public RoulettePrize Prize { get; set; } = new();
    public int NewBalance { get; set; }
}

public class BattlePassDto
{
    public List<BattlePassLevel> Levels { get; set; } = [];
    public List<BattlePassTask> Tasks { get; set; } = [];
    public BattlePassUserPoints? UserProgress { get; set; }
    public int Season { get; set; }
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

public class AdminUserDto : UserDto
{
    public string? UserIp { get; set; }
    public int Leave { get; set; }
    public DateTime? LeaveDate { get; set; }
}

public class UpdateUserStatusRequest
{
    public short Status { get; set; }
    public string? Reason { get; set; }
}

public class CreateNewsRequest
{
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string? ImageUrl { get; set; }
    public string? Slug { get; set; }
    public bool Published { get; set; }
}

public class CreateRedeemCodeRequest
{
    public string Code { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int MaxUses { get; set; }
    public DateTime? ExpiresAt { get; set; }
    public List<RedeemCodeItemRequest> Items { get; set; } = [];
}

public class RedeemCodeItemRequest
{
    public short ItemType { get; set; }
    public short TypeId { get; set; }
    public int Amount { get; set; }
    public int Points { get; set; }
}
