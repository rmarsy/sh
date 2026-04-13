# Sh — Blazor / ASP.NET Core Web API

A Blazor Web App + ASP.NET Core Web API conversion of the [shinertia](https://github.com/rmarsy/shinertia) PHP project.

---

## Architecture

| Project | Description |
|---------|-------------|
| `Sh.Api` | ASP.NET Core 10 Web API — all business logic, JWT auth, launcher endpoints |
| `Sh.Web` | Blazor Web App (Interactive Server) — frontend, calls the API |

---

## Requirements

- .NET 10 SDK
- SQL Server with the following databases already set up:
  - `PS_UserData` — users
  - `PS_GameData` — characters, items
  - `PS_GameLog` — boss logs
  - `PS_WebSite` — shop, news, roulette, redeem codes, etc.

---

## Configuration

### Sh.Api — `appsettings.json`

| Key | Description |
|-----|-------------|
| `ConnectionStrings.*` | Four connection strings, one per database |
| `Jwt:Key` | **Must change** — a random string ≥ 32 characters |
| `Jwt:ExpiryMinutes` | Token lifetime (default 120) |
| `Turnstile:SiteKey` | Cloudflare Turnstile site key |
| `Turnstile:SecretKey` | Cloudflare Turnstile secret key |
| `Turnstile:Enabled` | Set to `false` to disable Turnstile |
| `Cors:AllowedOrigins` | Array of allowed origins for CORS |
| `ApiKey:Keys` | **Must change** — array of valid API keys for the launcher |
| `PayPal:ClientId/ClientSecret` | PayPal credentials |
| `Stripe:PublishableKey/SecretKey/WebhookSecret` | Stripe credentials |

### Sh.Web — `appsettings.json`

| Key | Description |
|-----|-------------|
| `ApiBaseUrl` | URL of the running `Sh.Api` project |

---

## Setup

1. **Clone and restore**
   ```bash
   git clone <repo>
   cd sh
   dotnet restore
   ```

2. **Configure the API**

   Edit `Sh.Api/appsettings.json`:
   - Set all four `ConnectionStrings` to point at your SQL Server
   - Replace `Jwt:Key` with a secure random value
   - Add your Cloudflare Turnstile keys (get them at <https://dash.cloudflare.com/?to=/:account/turnstile>)
   - Replace `ApiKey:Keys[0]` with a secure random value for the launcher

3. **Configure the Blazor frontend**

   Edit `Sh.Web/appsettings.json`:
   - Set `ApiBaseUrl` to your API URL (e.g., `https://api.yourdomain.com`)
   - Add your Cloudflare Turnstile **site key** (used client-side in Register / Login forms)

4. **Run both projects**
   ```bash
   # Terminal 1
   cd Sh.Api && dotnet run

   # Terminal 2
   cd Sh.Web && dotnet run
   ```

---

## Security Features

- **JWT tokens** — issued by the API, stored as HttpOnly cookies in the Blazor app
- **Rate limiting** — 5 requests/minute on all `/api/auth/*` endpoints; 60/minute general
- **CORS** — only origins listed in `Cors:AllowedOrigins` may call the API
- **API key** — launcher must send `X-Api-Key: <key>` on all `/api/launcher/*` endpoints
- **Cloudflare Turnstile** — required on Register (and optionally Login)
- **Security headers** — `X-Content-Type-Options`, `X-Frame-Options`, `Referrer-Policy`, `Permissions-Policy`
- **HTTPS redirection** — enforced in production
- **Antiforgery tokens** — used on all Blazor forms

---

## Images

The original PHP project references game images (item icons, character images, boss images).  
Copy these into `Sh.Web/wwwroot/images/` with the same relative paths used in the PHP project's public folder.

Typical paths expected by the Blazor app:
- `/images/items/{TypeId}/{TypeId}.png` — game item icons
- `/images/characters/{family}/{job}/{sex}.png` — character previews
- `/images/boss/{mobId}.png` — boss images
- `/images/logo.png` — site logo

---

## Launcher API

The launcher can call protected endpoints under `/api/launcher/*` using an API key:

```http
GET /api/launcher/server-status
X-Api-Key: YOUR_API_KEY
```

Available launcher endpoints:
- `GET /api/launcher/server-status` — online player counts per faction
- `POST /api/launcher/authenticate` — validate credentials, returns JWT
- `GET /api/launcher/patch-notes` — latest patch notes

---

## Admin Access

Users with `admin = 1` in the database automatically get admin access.  
Admin level (`adminlevel`) controls granular permissions:
- `0` — regular user
- `1` — GM (game master)
- `2` — Admin

---

## Notes

- No EF Core migrations are included — the database schema is assumed to already exist.
- The app uses four separate `DbContext` instances, each pointing to a different SQL Server database.
- Passwords are hashed with BCrypt (same as the original PHP project).
