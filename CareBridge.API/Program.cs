using CareBridge.Api.Logic;

var builder = WebApplication.CreateBuilder(args);

// --- 1. Configuration Constants ---
const string AngularPolicy = "AllowAngularOrigin";
var allowedOrigin = builder.Configuration["CorsSettings:AllowedOrigin"];
if (string.IsNullOrEmpty(allowedOrigin))
{
    allowedOrigin = "http://localhost:4200"; // Default fallback
}

// --- 2. Service Registration (Dependency Injection) ---
builder.Services.AddControllers();
builder.Services.AddOpenApi();

// Register Business Logic
builder.Services.AddScoped<Engine>();

// Configure CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy(AngularPolicy, policy =>
        policy.WithOrigins(allowedOrigin)
              .AllowAnyMethod()
              .AllowAnyHeader());
});

var app = builder.Build();

// --- 3. Middleware Pipeline ---

// Enable Documentation in Development
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}
else
{
    app.UseHttpsRedirection();
}

app.UseCors(AngularPolicy);

app.UseAuthorization();

app.MapControllers();

app.Run();
