namespace Sh.Api.Middleware;

public class ApiKeyMiddleware(RequestDelegate next, IConfiguration configuration)
{
    public async Task InvokeAsync(HttpContext context)
    {
        if (context.Request.Path.StartsWithSegments("/api/launcher"))
        {
            var headerName = configuration["ApiKey:HeaderName"] ?? "X-Api-Key";
            var validKeys = configuration.GetSection("ApiKey:Keys").Get<string[]>() ?? [];

            if (!context.Request.Headers.TryGetValue(headerName, out var providedKey) ||
                !validKeys.Contains(providedKey.ToString()))
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsJsonAsync(new { message = "Invalid API key." });
                return;
            }
        }

        await next(context);
    }
}
