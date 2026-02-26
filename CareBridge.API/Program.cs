using CareBridge.Api.Data;
using CareBridge.Api.Settings;
using CareBridge.Api.SignalR;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// --- 1. CONFIGURATION ---
// Get data from the environment/platform.
var screeningSettings = builder.Configuration
    .GetSection("ScreeningSettings")
    .Get<ScreeningSettings>() ?? throw new Exception("Critical Error: 'ScreeningSettings' section is missing!");

const string AngularPolicy = "AllowAngularOrigin";
const string AngularUrl = "http://localhost:4200";
const string DbName = "carebridge.db";

// --- 2. SERVICES ---
builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddSignalR();

// Register the DATA so the Controller can pass it to our Static Logic.
builder.Services.AddSingleton(screeningSettings);

// Register the DATABASE.
builder.Services.AddDbContext<CareBridgeDbContext>(options =>
    options.UseSqlite($"Data Source={DbName}"));

// --- 3. CORS CONFIGURATION ---
builder.Services.AddCors(options =>
{
    options.AddPolicy(AngularPolicy, policy =>
        policy.WithOrigins(AngularUrl)
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials());
});

var app = builder.Build();

// --- 4. MIDDLEWARE PIPELINE ---
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseCors(AngularPolicy);
app.UseAuthorization();
app.MapControllers();

app.MapHub<PatientHub>("/patientHub");

// --- 5. DATA INITIALIZATION ---
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<CareBridgeDbContext>();

    // Wipe and recreate for a clean, predictable state every run.
    context.Database.EnsureDeleted();
    context.Database.EnsureCreated();

    Console.WriteLine($"---> Database Initialized: {context.Patients.Count()} patients loaded.");
}

app.Run();
