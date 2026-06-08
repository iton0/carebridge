# CareBridge — CLAUDE.md

## Project

CDS mini-service. Finds patients overdue for screenings. Clinician dashboard shows them.

## Stack

- **API:** ASP.NET Core 10, EF Core + SQLite, JWT, SignalR — `CareBridge.API/`
- **UI:** Angular 21 + Angular Material, SignalR client — `CareBridge-UI/`
- **Runtime mgr:** mise (`mise install` from root gets all deps)

## Ports

| Service | Port |
|---------|------|
| UI | 4200 |
| API | 5138 |

## Commands

```sh
make            # kill ports → start both (parallel)
make setup      # npm install + dotnet restore
make clean      # kill ports 4200 + 5138

cd CareBridge-UI && npx ng serve --port 4200
cd CareBridge.API && dotnet watch run --urls="http://localhost:5138"
```

## Key Files

```
CareBridge.API/
  Program.cs                        # bootstrap, DB wipe+seed on every run
  Logic/Screening.cs                # ScreeningLogic — threshold = cutoffDate - 1yr
  Settings/ScreeningSettings.cs     # CutoffDate from appsettings
  Controllers/PatientController.cs  # GET /api/patient/overdue, POST /api/patient
  Controllers/AuthController.cs     # POST /api/auth/login (admin/password hardcoded)
  SignalR/PatientHub.cs             # /patientHub — PatientOverdueAlert broadcast
  Data/DbContext.cs + Seed.cs       # SQLite context + seed data
  Models/Patient.cs                 # Patient entity; LastScreeningDate sentinel = DateOnly.MinValue

CareBridge-UI/src/app/
  dashboard/     # main clinician view
  login/         # login form
  services/      # auth.ts, patient.ts, signalr.ts
  guards/        # auth-guard.ts
  interceptors/  # auth-interceptor.ts (attaches JWT), logging-interceptor.ts
```

## Critical Behaviors

- **DB wiped on every startup** — `EnsureDeleted()` + `EnsureCreated()` in `Program.cs`. No persistence between runs.
- **Auth hardcoded** — `admin` / `password`. JWT key in `appsettings.Development.json`.
- **JWT expiry** — 1 min in dev (`DurationInMinutes: 1`).
- **CORS** — hardcoded to `http://localhost:4200`.
- **`GET /api/patient/overdue`** — no `[Authorize]` attribute; public endpoint.
- **SignalR alert** — fires `PatientOverdueAlert` only when POST `/api/patient` adds overdue patient.

## Screening Logic

```csharp
threshold = settings.CutoffDate.AddYears(-1);
overdue   = patient.LastScreeningDate < threshold;
```

Dev cutoff: `2026-01-01` → threshold `2025-01-01`. Patient overdue if screened before that.

## Config

`CareBridge.API/appsettings.Development.json` — override `ScreeningSettings.CutoffDate`, JWT key/issuer/audience/expiry.

## Testing

```sh
cd CareBridge-UI && npx ng test   # vitest
cd CareBridge.API && dotnet test  # (if test project added)
```

## Formatting

- UI: Prettier — `printWidth: 100`, `singleQuote: true`
- API: standard C# conventions
