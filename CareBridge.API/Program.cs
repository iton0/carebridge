using CareBridge.Api.Data;
using CareBridge.Api.Logic;
using CareBridge.Api.Settings;
using CareBridge.Api.SignalR;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var screeningSettings = builder.Configuration
    .GetSection("ScreeningSettings")
    .Get<ScreeningSettings>();

if (screeningSettings == null)
{
    throw new Exception("Critical Error: 'ScreeningSettings' section is missing from appsettings.json!");
}

const string AngularPolicy = "AllowAngularOrigin";
const string AngularUrl = "http://localhost:4200";
const string DbName = "carebridge.db";

// 1. Register Services
builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddSingleton(screeningSettings);
builder.Services.AddScoped<IEngine, Engine>();
builder.Services.AddSignalR();
builder.Services.AddDbContext<CareBridgeDbContext>(options =>
    options.UseSqlite($"Data Source={DbName}"));

// 2. Configure CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy(AngularPolicy, policy =>
        policy.WithOrigins(AngularUrl)
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials());
});

var app = builder.Build();

// 3. Configure Middleware
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseCors(AngularPolicy);
app.UseAuthorization();
app.MapControllers();

// 4. Map the Real-time Hub
app.MapHub<PatientHub>("/patientHub");

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<CareBridgeDbContext>();

    // This deletes the DB and recreates it with ONLY seed data 
    // every time the app starts.
    context.Database.EnsureDeleted();
    context.Database.EnsureCreated();

    var count = context.Patients.Count();
    Console.WriteLine($"---> Database check: {count} patients found in SQLite.");
}

app.Run();
