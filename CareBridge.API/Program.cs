using CareBridge.Api.Logic;
using CareBridge.Api.SignalR;

var builder = WebApplication.CreateBuilder(args);

const string AngularPolicy = "AllowAngularOrigin";

// 1. Register Services
builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddScoped<Engine>();
builder.Services.AddSignalR();

// 2. Configure CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy(AngularPolicy, policy =>
        policy.WithOrigins("http://localhost:4200")
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

app.Run();
