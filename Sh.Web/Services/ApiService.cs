using System.Net.Http.Json;
using System.Text.Json;
using Sh.Web.Models;

namespace Sh.Web.Services;

public class ApiService(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
{
    private static readonly JsonSerializerOptions JsonOptions = new() { PropertyNameCaseInsensitive = true };

    private HttpClient CreateClient()
    {
        var client = httpClientFactory.CreateClient("Api");
        var token = httpContextAccessor.HttpContext?.Request.Cookies["sh_token"];
        if (!string.IsNullOrEmpty(token))
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        return client;
    }

    public async Task<ApiResponse<T>?> GetAsync<T>(string path)
    {
        try
        {
            var response = await CreateClient().GetAsync(path);
            return await response.Content.ReadFromJsonAsync<ApiResponse<T>>(JsonOptions);
        }
        catch
        {
            return null;
        }
    }

    public async Task<ApiResponse<T>?> PostAsync<T>(string path, object? body = null)
    {
        try
        {
            var response = await CreateClient().PostAsJsonAsync(path, body);
            return await response.Content.ReadFromJsonAsync<ApiResponse<T>>(JsonOptions);
        }
        catch
        {
            return null;
        }
    }

    public async Task<ApiResponse<T>?> PutAsync<T>(string path, object? body = null)
    {
        try
        {
            var response = await CreateClient().PutAsJsonAsync(path, body);
            return await response.Content.ReadFromJsonAsync<ApiResponse<T>>(JsonOptions);
        }
        catch
        {
            return null;
        }
    }

    public async Task<ApiResponse<T>?> DeleteAsync<T>(string path)
    {
        try
        {
            var response = await CreateClient().DeleteAsync(path);
            return await response.Content.ReadFromJsonAsync<ApiResponse<T>>(JsonOptions);
        }
        catch
        {
            return null;
        }
    }

    public async Task<(bool Success, string Message, LoginResponse? Data)> LoginAsync(string username, string password, string? turnstileToken)
    {
        var result = await PostAsync<LoginResponse>("api/auth/login", new { username, password, turnstileToken });
        if (result == null) return (false, "Connection error.", null);
        return (result.Success, result.Message ?? string.Empty, result.Data);
    }

    public async Task<(bool Success, string Message)> RegisterAsync(string username, string password, string turnstileToken)
    {
        var result = await PostAsync<object>("api/auth/register", new { username, password, turnstileToken });
        if (result == null) return (false, "Connection error.");
        return (result.Success, result.Message ?? string.Empty);
    }

    public async Task<(bool Success, string Message)> ForgotPasswordAsync(string username, string securityAnswer, string turnstileToken)
    {
        var result = await PostAsync<object>("api/auth/forgot-password", new { username, securityAnswer, turnstileToken });
        if (result == null) return (false, "Connection error.");
        return (result.Success, result.Message ?? "If an account was found, a reset link has been sent.");
    }

    public async Task<(bool Success, string Message)> ResetPasswordAsync(string token, string newPassword)
    {
        var result = await PostAsync<object>("api/auth/reset-password", new { token, newPassword });
        if (result == null) return (false, "Connection error.");
        return (result.Success, result.Message ?? string.Empty);
    }

    public async Task<DashboardDto?> GetDashboardAsync()
    {
        var result = await GetAsync<DashboardDto>("api/dashboard");
        return result?.Data;
    }

    public async Task<HomeDto?> GetHomeAsync()
    {
        var result = await GetAsync<HomeDto>("api/public/home");
        return result?.Data;
    }

    public async Task<PagedResult<NewsDto>?> GetNewsAsync(int page = 1, int pageSize = 10, string? category = null)
    {
        var url = $"api/public/news?page={page}&pageSize={pageSize}";
        if (!string.IsNullOrEmpty(category)) url += $"&category={Uri.EscapeDataString(category)}";
        var result = await GetAsync<PagedResult<NewsDto>>(url);
        return result?.Data;
    }

    public async Task<PagedResult<CharacterDto>?> GetPvpRankingAsync(int page = 1, int pageSize = 20)
    {
        var result = await GetAsync<PagedResult<CharacterDto>>($"api/public/pvp-ranking?page={page}&pageSize={pageSize}");
        return result?.Data;
    }

    public async Task<ShopDto?> GetShopAsync()
    {
        var result = await GetAsync<ShopDto>("api/shop");
        return result?.Data;
    }

    public async Task<(bool Success, string Message, int NewBalance)> PurchaseAsync(int itemId, int characterId, int amount)
    {
        var result = await PostAsync<PurchaseResult>("api/shop/purchase", new { itemId, characterId, amount });
        if (result == null) return (false, "Connection error.", 0);
        return (result.Success, result.Message ?? string.Empty, result.Data?.NewBalance ?? 0);
    }

    public async Task<RouletteDto?> GetRouletteAsync()
    {
        var result = await GetAsync<RouletteDto>("api/roulette");
        return result?.Data;
    }

    public async Task<(bool Success, string Message, SpinResultDto? Result)> SpinAsync(int rouletteId, int characterId)
    {
        var result = await PostAsync<SpinResultDto>("api/roulette/spin", new { rouletteId, characterId });
        if (result == null) return (false, "Connection error.", null);
        return (result.Success, result.Message ?? string.Empty, result.Data);
    }

    public async Task<(bool Success, string Message)> RedeemCodeAsync(string code, int characterId)
    {
        var result = await PostAsync<object>("api/redeemcode/redeem", new { code, characterId });
        if (result == null) return (false, "Connection error.");
        return (result.Success, result.Message ?? string.Empty);
    }

    public async Task<VoteDto?> GetVoteStatusAsync()
    {
        var result = await GetAsync<VoteDto>("api/vote");
        return result?.Data;
    }

    public async Task<(bool Success, string Message, int NewBalance)> ClaimVoteAsync(string voteSite)
    {
        var result = await PostAsync<VoteClaimResult>("api/vote/claim", new { voteSite });
        if (result == null) return (false, "Connection error.", 0);
        return (result.Success, result.Message ?? string.Empty, result.Data?.NewBalance ?? 0);
    }

    public async Task<BattlePassDto?> GetBattlePassAsync()
    {
        var result = await GetAsync<BattlePassDto>("api/battlepass");
        return result?.Data;
    }

    public async Task<List<PaymentDto>?> GetPaymentHistoryAsync()
    {
        var result = await GetAsync<List<PaymentDto>>("api/donate/history");
        return result?.Data;
    }

    public async Task<ProfileDto?> GetProfileAsync()
    {
        var result = await GetAsync<ProfileDto>("api/profile");
        return result?.Data;
    }

    public async Task<(bool Success, string Message)> ChangePasswordAsync(string currentPassword, string newPassword)
    {
        var result = await PutAsync<object>("api/profile/password", new { currentPassword, newPassword });
        if (result == null) return (false, "Connection error.");
        return (result.Success, result.Message ?? string.Empty);
    }

    public async Task<PagedResult<AdminUserDto>?> GetAdminUsersAsync(int page = 1, string? search = null)
    {
        var url = $"api/admin/users?page={page}";
        if (!string.IsNullOrEmpty(search)) url += $"&search={Uri.EscapeDataString(search)}";
        var result = await GetAsync<PagedResult<AdminUserDto>>(url);
        return result?.Data;
    }

    public async Task<PagedResult<NewsDto>?> GetAdminNewsAsync(int page = 1)
    {
        var result = await GetAsync<PagedResult<NewsDto>>($"api/admin/news?page={page}");
        return result?.Data;
    }

    public async Task<PagedResult<RedeemCodeDto>?> GetAdminRedeemCodesAsync(int page = 1)
    {
        var result = await GetAsync<PagedResult<RedeemCodeDto>>($"api/admin/redeem-codes?page={page}");
        return result?.Data;
    }

    public async Task<PagedResult<ShopItemDto>?> GetAdminShopItemsAsync(int page = 1)
    {
        var result = await GetAsync<PagedResult<ShopItemDto>>($"api/admin/shop-items?page={page}");
        return result?.Data;
    }

    public async Task<List<SettingDto>?> GetSettingsAsync()
    {
        var result = await GetAsync<List<SettingDto>>("api/admin/settings");
        return result?.Data;
    }
}
