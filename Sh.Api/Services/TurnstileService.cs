using System.Text.Json;

namespace Sh.Api.Services;

public class TurnstileService(IHttpClientFactory httpClientFactory, IConfiguration configuration)
{
    private const string VerifyUrl = "https://challenges.cloudflare.com/turnstile/v0/siteverify";

    public async Task<bool> VerifyAsync(string token, string? remoteIp = null)
    {
        var enabled = configuration.GetValue<bool>("Turnstile:Enabled");
        if (!enabled) return true;

        var secretKey = configuration["Turnstile:SecretKey"];
        var client = httpClientFactory.CreateClient();

        var formData = new Dictionary<string, string>
        {
            ["secret"] = secretKey!,
            ["response"] = token
        };

        if (!string.IsNullOrEmpty(remoteIp))
            formData["remoteip"] = remoteIp;

        var response = await client.PostAsync(VerifyUrl, new FormUrlEncodedContent(formData));
        var json = await response.Content.ReadAsStringAsync();
        var result = JsonDocument.Parse(json);
        return result.RootElement.GetProperty("success").GetBoolean();
    }
}
